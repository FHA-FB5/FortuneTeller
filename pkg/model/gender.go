package model

import (
	"database/sql/driver"
	"fmt"
	"strings"
)

type Gender string

func (g *Gender) Scan(src interface{}) error {
	v := Gender(strings.ToLower(fmt.Sprintf("%v", src)))
	switch v {
	case GenderNotSpecified, GenderFemale, GenderMale:
		*g = v
		return nil
	default:
		return fmt.Errorf("invalid Gender: %v", v)
	}
}

func (g Gender) Value() (driver.Value, error) {
	switch g {
	case GenderNotSpecified, GenderFemale, GenderMale:
		return string(g), nil
	default:
		return nil, fmt.Errorf("invalid gender: %s", g)
	}
}

const (
	GenderMale         Gender = "m"
	GenderFemale              = "f"
	GenderNotSpecified        = "d"
)
