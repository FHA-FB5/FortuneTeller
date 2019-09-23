package model

import (
	"database/sql/driver"
	"fmt"
	"strings"
)

type Major string

func (m Major) Value() (driver.Value, error) {
	switch m {
	case MajorBusinessInformationSystems, MajorElectricalEngineering, MajorMediaAndCommunicationForDigitalBusiness, MajorComputerScience:
		return string(m), nil
	default:
		return nil, fmt.Errorf("invalid major: %s", m)
	}
}

func (m *Major) Scan(src interface{}) error {
	v := Major(strings.ToUpper(fmt.Sprintf("%v", src)))
	switch v {
	case MajorBusinessInformationSystems, MajorElectricalEngineering, MajorMediaAndCommunicationForDigitalBusiness, MajorComputerScience:
		*m = v
	default:
		return fmt.Errorf("invalid major: %s", v)
	}
	return nil
}

const (
	MajorComputerScience                         Major = "INF"
	MajorElectricalEngineering                         = "ET"
	MajorMediaAndCommunicationForDigitalBusiness       = "MCD"
	MajorBusinessInformationSystems                    = "WI"
)

var (
	Majors = []Major{
		MajorComputerScience,
		MajorElectricalEngineering,
		MajorMediaAndCommunicationForDigitalBusiness,
		MajorBusinessInformationSystems,
	}
)
