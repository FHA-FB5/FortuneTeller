package database

import (
	"context"
	"io"

	"github.com/jmoiron/sqlx"
	uuid "github.com/satori/go.uuid"

	"github.com/FHA-FB5/FortuneTeller/pkg/model"
)

const (
	getGroup = `
		select * from groups where id = $1 and deleted_at is null
	`

	updateGroup = `
		update groups set
			name=:name
		where id=:id and deleted_at is null
	`

	createGroup = `
		insert into groups(id, created_at, name) values (
			:id,
			now(),
			:name
		)
	`

	deleteGroup = `
		update groups set deleted_at = now() where id = $1 and deleted_at is null
	`

	listGroups = `
		select * from groups where deleted_at is null
	`
)

type GroupRepository struct {
	db     *sqlx.DB
	get    *sqlx.Stmt
	update *sqlx.NamedStmt
	create *sqlx.NamedStmt
	delete *sqlx.Stmt
	list   *sqlx.Stmt
}

func NewGroupRepository(ctx context.Context, db *sqlx.DB) (*GroupRepository, error) {
	if db == nil {
		return nil, ErrDBNil
	}
	get, err := db.PreparexContext(ctx, getGroup)
	if err != nil {
		return nil, err
	}
	update, err := db.PrepareNamedContext(ctx, updateGroup)
	if err != nil {
		return nil, err
	}
	create, err := db.PrepareNamedContext(ctx, createGroup)
	if err != nil {
		return nil, err
	}
	deleteGroup, err := db.PreparexContext(ctx, deleteGroup)
	if err != nil {
		return nil, err
	}
	list, err := db.PreparexContext(ctx, listGroups)
	return &GroupRepository{
		db:     db,
		get:    get,
		update: update,
		create: create,
		delete: deleteGroup,
		list:   list,
	}, nil
}

func (r GroupRepository) Close() error {
	var final error
	for _, stmt := range []io.Closer{r.get, r.update, r.create, r.delete} {
		if err := stmt.Close(); err != nil {
			final = err
		}
	}
	return final
}

func (r GroupRepository) Get(ctx context.Context, id string) (*model.Group, error) {
	var res model.Group
	if err := r.get.GetContext(ctx, &res, id); err != nil {
		return nil, err
	}
	return &res, nil
}

func (r GroupRepository) Update(ctx context.Context, group model.Group) error {
	if err := txScope(ctx, r.db, func(tx *sqlx.Tx) error {
		res, err := tx.NamedStmt(r.update).ExecContext(ctx, group)
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

func (r GroupRepository) Create(ctx context.Context, group model.Group) (*model.Group, error) {
	group.ID = uuid.NewV4().String()
	if err := txScope(ctx, r.db, func(tx *sqlx.Tx) error {
		_, err := tx.NamedStmt(r.create).ExecContext(ctx, group)
		if err != nil {
			return err
		}
		return nil
	}); err != nil {
		return nil, err
	}
	return r.Get(ctx, group.ID)
}

func (r GroupRepository) Delete(ctx context.Context, id string) error {
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

func (r GroupRepository) List(ctx context.Context) ([]*model.Group, error) {
	res := []*model.Group{}
	if err := r.list.SelectContext(ctx, &res); err != nil {
		return nil, err
	}
	return res, nil
}
