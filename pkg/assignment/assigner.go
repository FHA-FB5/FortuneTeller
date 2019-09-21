package assignment

import (
	"fmt"
	"sort"

	"github.com/FHA-FB5/FortuneTeller/pkg/model"
)

type Assigner struct {
	groups      []*group
	personCount int
}

func NewAssigner(input []*model.Group) *Assigner {
	assigner := &Assigner{
		groups: make([]*group, 0, len(input)),
	}
	for _, g := range input {
		_ = assigner.AddGroup(g)
	}
	return assigner
}

func (a *Assigner) Assign(person *model.Person) (string, error) {
	if person == nil {
		return "", fmt.Errorf("person can not be nil")
	}
	if len(a.groups) == 0 {
		return "", fmt.Errorf("no groups to assign to")
	}
	maxCount := a.personCount / len(a.groups)
	var groups []*group
	for _, group := range a.groups {
		if group != nil && group.Members <= maxCount {
			groups = append(groups, group)
		}
	}
	if len(groups) < 1 {
		return "", fmt.Errorf("no groups to assign to")
	}
	sort.Slice(groups, func(i, j int) bool {
		return groups[i].Diff(*person) > groups[j].Diff(*person)
	})
	a.personCount++
	for i, group := range a.groups {
		if group.ID == groups[0].ID {
			a.groups[i].Add(*person)
		}
	}
	return groups[0].ID, nil
}

func (a *Assigner) AddGroup(g *model.Group) error {
	if g == nil {
		return fmt.Errorf("group can not be nil")
	}
	group := newGroup(g.ID)
	for _, member := range g.Members {
		if member != nil {
			a.personCount++
			group.Add(*member)
		}
	}
	a.groups = append(a.groups, group)
	return nil
}
