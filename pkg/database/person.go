package database

import (
	"context"
	"errors"
	"io"

	"github.com/jmoiron/sqlx"
	uuid "github.com/satori/go.uuid"

	"github.com/FHA-FB5/FortuneTeller/pkg/model"
)

const (
	getPerson = `
		select * from persons where id = $1 and deleted_at is null
	`

	updatePerson = `
		update persons set
			group_id=:group_id,
			surname=:surname,
			name=:name,
			major=:major,
			gender=:gender,
			age=:age
		where id = :id and deleted_at is null
	`

	createPerson = `
		insert into persons(id, created_at, surname, name, major, gender, age) values (
			:id,
		    now(),
		    :surname,
		    :name,
		    :major,
		    :gender,
		    :age
		)
	`

	deletePerson = `
		update persons set deleted_at = now() where id = $1 and deleted_at is null
	`

	listPersonsByGroupID = `
		select * from persons where group_id = $1 and deleted_at is null
	`
)

var (
	ErrNotExists = errors.New("resource does not exist")
	ErrDBNil     = errors.New("database is nil")
)

type PersonRepository struct {
	db          *sqlx.DB
	get         *sqlx.Stmt
	update      *sqlx.NamedStmt
	create      *sqlx.NamedStmt
	delete      *sqlx.Stmt
	listByGroup *sqlx.Stmt
}

func NewPersonRepository(ctx context.Context, db *sqlx.DB) (*PersonRepository, error) {
	if db == nil {
		return nil, ErrDBNil
	}
	get, err := db.PreparexContext(ctx, getPerson)
	if err != nil {
		return nil, err
	}
	update, err := db.PrepareNamedContext(ctx, updatePerson)
	if err != nil {
		return nil, err
	}
	create, err := db.PrepareNamedContext(ctx, createPerson)
	if err != nil {
		return nil, err
	}
	deletePerson, err := db.PreparexContext(ctx, deletePerson)
	if err != nil {
		return nil, err
	}
	listByGroup, err := db.PreparexContext(ctx, listPersonsByGroupID)
	if err != nil {
		return nil, err
	}
	return &PersonRepository{
		db:          db,
		get:         get,
		update:      update,
		create:      create,
		delete:      deletePerson,
		listByGroup: listByGroup,
	}, nil
}

func (r PersonRepository) Close() error {
	var final error
	for _, stmt := range []io.Closer{r.get, r.update, r.create, r.delete, r.listByGroup} {
		if err := stmt.Close(); err != nil {
			final = err
		}
	}
	return final
}

func (r PersonRepository) Get(ctx context.Context, id string) (*model.Person, error) {
	var res model.Person
	if err := r.get.GetContext(ctx, &res, id); err != nil {
		return nil, err
	}
	return &res, nil
}

func (r PersonRepository) Update(ctx context.Context, person model.Person) error {
	if err := txScope(ctx, r.db, func(tx *sqlx.Tx) error {
		res, err := tx.NamedStmt(r.update).ExecContext(ctx, person)
		if err != nil {
			return err
		}
		n, err := res.RowsAffected()
		if err != nil {
			return err
		}
		if n == 0 {
			return ErrNotExists
		}
		return nil
	}); err != nil {
		return err
	}
	return nil
}

func (r PersonRepository) Create(ctx context.Context, person model.Person) (*model.Person, error) {
	person.ID = uuid.NewV4().String()
	if err := txScope(ctx, r.db, func(tx *sqlx.Tx) error {
		_, err := tx.NamedStmt(r.create).ExecContext(ctx, person)
		if err != nil {
			return err
		}
		return nil
	}); err != nil {
		return nil, err
	}
	return r.Get(ctx, person.ID)
}

func (r PersonRepository) Delete(ctx context.Context, id string) error {
	if err := txScope(ctx, r.db, func(tx *sqlx.Tx) error {
		res, err := tx.Stmtx(r.delete).ExecContext(ctx, id)
		if err != nil {
			return err
		}
		n, err := res.RowsAffected()
		if err != nil {
			return err
		}
		if n == 0 {
			return ErrNotExists
		}
		return nil
	}); err != nil {
		return err
	}
	return nil
}

func (r PersonRepository) ListByGroup(ctx context.Context, groupID string) ([]*model.Person, error) {
	res := []*model.Person{}
	if err := r.listByGroup.SelectContext(ctx, &res, groupID); err != nil {
		return nil, err
	}
	return res, nil
}
