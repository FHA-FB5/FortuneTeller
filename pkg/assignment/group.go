package assignment

import "github.com/FHA-FB5/FortuneTeller/pkg/model"

type group struct {
	Age     *ageScore
	Gender  *genderScore
	Major   *majorScore
	Members int
	ID      string
}

func newGroup(id string) *group {
	return &group{
		Age:    &ageScore{},
		Gender: newGenderScore(),
		Major:  newMajorScore(),
		ID:     id,
	}
}

func (g *group) Add(person model.Person) {
	g.Members++
	g.Age.Add(person.Age)
	g.Gender.Add(person.Gender)
	g.Major.Add(person.Major)
}

func (g group) Score() float64 {
	return g.Gender.Score() + g.Major.Score() + g.Age.Score()
}

func (g group) Diff(person model.Person) float64 {
	return g.Gender.Diff(person.Gender) + g.Major.Diff(person.Major) + g.Age.Diff(person.Age)
}
