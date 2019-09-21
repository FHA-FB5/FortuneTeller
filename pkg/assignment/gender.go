package assignment

import (
	"math"

	"github.com/FHA-FB5/FortuneTeller/pkg/model"
)

var (
	genders           = []model.Gender{model.GenderFemale, model.GenderMale}
	genderTargetShare = map[model.Gender]float64{
		model.GenderFemale: .2,
		model.GenderMale:   .8,
	}
)

type genderScore struct {
	counts map[model.Gender]int
}

func newGenderScore() *genderScore {
	counts := map[model.Gender]int{}
	for _, gender := range genders {
		counts[gender] = 0
	}
	return &genderScore{
		counts: counts,
	}
}

func (s genderScore) totalCount() int {
	total := 0
	for _, gender := range genders {
		total += s.counts[gender]
	}
	return total
}

func (s genderScore) share(gender model.Gender) float64 {
	return float64(s.counts[gender]) / float64(s.totalCount())
}

func (s genderScore) Score() float64 {
	score := .0
	for _, gender := range genders {
		score += math.Abs(genderTargetShare[gender] - s.share(gender))
	}
	return score
}

func (s genderScore) Diff(gender model.Gender) float64 {
	before := s.Score()
	s.counts[gender]++
	after := s.Score()
	s.counts[gender]--
	return before - after
}

func (s *genderScore) Add(gender model.Gender) {
	s.counts[gender]++
}
