package model

import (
	"time"
)

type Person struct {
	ID        string    `json:"id" db:"id"`
	CreatedAt time.Time `json:"-" db:"created_at"`
	UpdatedAt time.Time `json:"-" db:"updated_at"`
	DeletedAt time.Time `json:"-" db:"deleted_at"`
	GroupID   string    `json:"groupId" db:"group_id"`
	Surname   string    `json:"surname" db:"surname"`
	Name      string    `json:"name" db:"name"`
	Major     Major     `json:"major" db:"major"`
	Gender    Gender    `json:"gender" db:"gender"`
	Age       uint      `json:"age" db:"age"`
}
