package assignment

import "math"

var (
	ageTarget = 22.0
)

type ageScore struct {
	Count uint
	Total uint
}

func (s ageScore) average() float64 {
	return float64(s.Total) / float64(s.Count)
}

func (s *ageScore) Add(age uint) {
	s.Count++
	s.Total += age
}

func (s ageScore) remove(age uint) {
	s.Count--
	s.Total -= age
}

func (s ageScore) Score() float64 {
	return math.Abs(ageTarget - s.average())
}

func (s ageScore) Diff(age uint) float64 {
	before := s.Score()
	s.Add(age)
	after := s.Score()
	s.remove(age)
	return before - after
}
