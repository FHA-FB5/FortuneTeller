package api

import (
	"context"
	"database/sql"
	"encoding/json"
	"errors"
	"fmt"
	"net/http"

	"github.com/gorilla/mux"
	"github.com/sirupsen/logrus"

	db "github.com/FHA-FB5/FortuneTeller/pkg/database"
	"github.com/FHA-FB5/FortuneTeller/pkg/model"
)

const (
	idPathParameter = "id"
)

var (
	ErrRepoNil = errors.New("repository is nil")
)

type PersonRepository interface {
	Get(ctx context.Context, id string) (*model.Person, error)
	Update(ctx context.Context, person model.Person) error
	Create(ctx context.Context, person model.Person) (*model.Person, error)
}

type Assigner interface {
	Assign(person *model.Person) (string, error)
}

type PersonService struct {
	logger   logrus.FieldLogger
	persons  PersonRepository
	assigner Assigner
}

func NewPersonService(repo PersonRepository, assigner Assigner, logger logrus.FieldLogger) (*PersonService, error) {
	if repo == nil {
		return nil, ErrRepoNil
	}
	if logger == nil {
		logger = logrus.StandardLogger()
	}
	return &PersonService{
		logger:   logger.WithField("service", "person"),
		assigner: assigner,
		persons:  repo,
	}, nil
}

func (s PersonService) Get(w http.ResponseWriter, r *http.Request) {
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
	person, err := s.persons.Get(r.Context(), id)
	if err != nil {
		if err == sql.ErrNoRows {
			writeError(w, http.StatusNotFound, fmt.Sprintf("person with id %s does not exist", id), logger)
			return
		}
		logger.Error(err)
		writeError(w, http.StatusInternalServerError, "could not get person", logger)
		return
	}
	if err := json.NewEncoder(w).Encode(person); err != nil {
		logger.Error(err)
	}
}

func (s PersonService) Update(w http.ResponseWriter, r *http.Request) {
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
	var person model.Person
	if err := json.NewDecoder(r.Body).Decode(&person); err != nil {
		writeError(w, http.StatusBadRequest, "could not read request body", logger)
		return
	}
	if err := s.persons.Update(r.Context(), person); err != nil {
		if err == db.ErrNotExists {
			writeError(w, http.StatusNotFound, fmt.Sprintf("person with id %s does not exist", id), logger)
			return
		}
		logger.Error(err)
		writeError(w, http.StatusInternalServerError, "could not update person", logger)
		return
	}
	if err := json.NewEncoder(w).Encode(person); err != nil {
		logger.Error(err)
	}
}

func (s PersonService) Create(w http.ResponseWriter, r *http.Request) {
	logger := s.logger.WithField("method", "create")
	var in model.Person
	if err := json.NewDecoder(r.Body).Decode(&in); err != nil {
		writeError(w, http.StatusBadRequest, "could not read request body", logger)
		return
	}
	groupID, err := s.assigner.Assign(&in)
	if err != nil {
		logger.Error(err)
		writeError(w, http.StatusInternalServerError, "could not assign person", logger)
		return
	}
	in.GroupID = groupID
	out, err := s.persons.Create(r.Context(), in)
	if err != nil {
		logger.Error(err)
		writeError(w, http.StatusInternalServerError, "could not create person", logger)
		return
	}
	w.WriteHeader(http.StatusCreated)
	if err := json.NewEncoder(w).Encode(out); err != nil {
		logger.Error(err)
	}
}
