package api

import (
	"encoding/json"
	"net/http"
	"regexp"

	"github.com/sirupsen/logrus"

	"github.com/FHA-FB5/FortuneTeller/pkg/model"
)

var (
	uuidRegex = regexp.MustCompile("[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}")
)

func writeError(w http.ResponseWriter, status int, message string, logger logrus.FieldLogger) {
	w.WriteHeader(status)
	if err := json.NewEncoder(w).Encode(model.Error{
		Message: message,
	}); err != nil {
		logger.Error(err)
	}
}
