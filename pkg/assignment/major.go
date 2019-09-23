package assignment

import (
	"math"

	"github.com/FHA-FB5/FortuneTeller/pkg/model"
)

var (
	majors = []model.Major{
		model.MajorBusinessInformationSystems,
		model.MajorComputerScience,
		model.MajorElectricalEngineering,
		model.MajorMediaAndCommunicationForDigitalBusiness,
	}
	majorTargetShare = map[model.Major]float64{
		model.MajorBusinessInformationSystems:              .1,
		model.MajorComputerScience:                         .6,
		model.MajorElectricalEngineering:                   .2,
		model.MajorMediaAndCommunicationForDigitalBusiness: .1,
	}
)

type majorScore struct {
	counts map[model.Major]int
}

func newMajorScore() *majorScore {
	counts := map[model.Major]int{}
	for _, major := range majors {
		counts[major] = 0
	}
	return &majorScore{
		counts: counts,
	}
}

func (s majorScore) totalCount() int {
	total := 0
	for _, major := range majors {
		total += s.counts[major]
	}
	return total
}

func (s majorScore) share(major model.Major) float64 {
	return float64(s.counts[major]) / float64(s.totalCount())
}

func (s majorScore) Score() float64 {
	score := .0
	for _, major := range majors {
		score += math.Abs(majorTargetShare[major] - s.share(major))
	}
	return score
}

func (s majorScore) Diff(major model.Major) float64 {
	before := s.Score()
	s.counts[major]++
	after := s.Score()
	s.counts[major]--
	return before - after
}

func (s *majorScore) Add(major model.Major) {
	s.counts[major]++
}
