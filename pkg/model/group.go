package model

import (
	"time"
)

type Group struct {
	ID        string    `json:"id" db:"id"`
	CreatedAt time.Time `json:"-" db:"created_at"`
	UpdatedAt time.Time `json:"-" db:"updated_at"`
	DeletedAt time.Time `json:"-" db:"deleted_at"`
	Name      string    `json:"name" db:"name"`
	Members   []*Person `json:"members" db:"-"`
}
