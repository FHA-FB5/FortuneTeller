package api

import (
	"context"
	"database/sql"
	"encoding/json"
	"fmt"
	"net/http"

	"github.com/gorilla/mux"
	"github.com/sirupsen/logrus"

	"github.com/FHA-FB5/FortuneTeller/pkg/database"
	"github.com/FHA-FB5/FortuneTeller/pkg/model"
)

type PersonLister interface {
	ListByGroup(ctx context.Context, groupID string) ([]*model.Person, error)
}

type GroupRepository interface {
	Get(ctx context.Context, id string) (*model.Group, error)
	Update(ctx context.Context, group model.Group) error
	Create(ctx context.Context, group model.Group) (*model.Group, error)
	List(ctx context.Context) ([]*model.Group, error)
}

type GroupAdder interface {
	AddGroup(g *model.Group) error
}

type GroupService struct {
	logger     logrus.FieldLogger
	persons    PersonLister
	groups     GroupRepository
	groupAdder GroupAdder
}

func NewGroupService(lister PersonLister, groups GroupRepository, adder GroupAdder, logger logrus.FieldLogger) (*GroupService, error) {
	if lister == nil || groups == nil {
		return nil, ErrRepoNil
	}
	if logger == nil {
		logger = logrus.StandardLogger()
	}
	return &GroupService{
		logger:     logger.WithField("service", "group"),
		persons:    lister,
		groups:     groups,
		groupAdder: adder,
	}, nil
}

func (s GroupService) List(w http.ResponseWriter, r *http.Request) {
	logger := s.logger.WithField("method", "list")
	groups, err := s.groups.List(r.Context())
	if err != nil {
		logger.Error(err)
		writeError(w, http.StatusInternalServerError, "could not get groups", logger)
		return
	}
	for i, group := range groups {
		members, err := s.persons.ListByGroup(r.Context(), group.ID)
		if err != nil {
			logger.Error(err)
			writeError(w, http.StatusInternalServerError, "could not get groups", logger)
			return
		}
		groups[i].Members = members
	}
	if err := json.NewEncoder(w).Encode(groups); err != nil {
		logger.Error(err)
	}
}

func (s GroupService) Get(w http.ResponseWriter, r *http.Request) {
	logger := s.logger.WithField("method", "get")
	id, ok := mux.Vars(r)[idPathParameter]
	if !ok {
		writeError(w, http.StatusBadRequest, "id could not be found in path", logger)
		return
	}
	if !uuidRegex.MatchString(id) {
		writeError(w, http.StatusBadRequest, "id is not a uuid", logger)
		return
	}
	group, err := s.groups.Get(r.Context(), id)
	if err != nil {
		if err == sql.ErrNoRows {
			writeError(w, http.StatusNotFound, fmt.Sprintf("group with id %s does not exist", id), logger)
			return
		}
		logger.Error(err)
		writeError(w, http.StatusInternalServerError, "could not get group", logger)
		return
	}
	if err := json.NewEncoder(w).Encode(group); err != nil {
		logger.Error(err)
	}
}

func (s GroupService) Update(w http.ResponseWriter, r *http.Request) {
	logger := s.logger.WithField("method", "update")
	id, ok := mux.Vars(r)[idPathParameter]
	if !ok {
		writeError(w, http.StatusBadRequest, "id could not be found in path", logger)
		return
	}
	if !uuidRegex.MatchString(id) {
		writeError(w, http.StatusBadRequest, "id is not a uuid", logger)
		return
	}
	var group model.Group
	if err := json.NewDecoder(r.Body).Decode(&group); err != nil {
		writeError(w, http.StatusBadRequest, "could not read request body", logger)
		return
	}
	if err := s.groups.Update(r.Context(), group); err != nil {
		if err == database.ErrNotExists {
			writeError(w, http.StatusNotFound, fmt.Sprintf("group with id %s does not exist", id), logger)
			return
		}
		logger.Error(err)
		writeError(w, http.StatusInternalServerError, "could not update group", logger)
		return
	}
	if err := json.NewEncoder(w).Encode(group); err != nil {
		logger.Error(err)
	}
}

func (s GroupService) Create(w http.ResponseWriter, r *http.Request) {
	logger := s.logger.WithField("method", "create")
	var in model.Group
	if err := json.NewDecoder(r.Body).Decode(&in); err != nil {
		writeError(w, http.StatusBadRequest, "could not read request body", logger)
		return
	}
	out, err := s.groups.Create(r.Context(), in)
	if err != nil {
		logger.Error(err)
		writeError(w, http.StatusInternalServerError, "could not create group", logger)
		return
	}
	if err := s.groupAdder.AddGroup(out); err != nil {
		logger.Error(err)
		writeError(w, http.StatusInternalServerError, "could not create group", logger)
		return
	}
	w.WriteHeader(http.StatusCreated)
	if err := json.NewEncoder(w).Encode(out); err != nil {
		logger.Error(err)
	}
}
