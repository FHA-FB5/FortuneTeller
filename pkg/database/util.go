package database

import (
	"context"

	"github.com/jmoiron/sqlx"
)

func txScope(ctx context.Context, db *sqlx.DB, f func(tx *sqlx.Tx) error) error {
	tx, err := db.BeginTxx(ctx, nil)
	if err != nil {
		return err
	}
	if err := f(tx); err != nil {
		if err := tx.Rollback(); err != nil {
			return err
		}
		return err
	} else if err := tx.Commit(); err != nil {
		return err
	}
	return nil
}
