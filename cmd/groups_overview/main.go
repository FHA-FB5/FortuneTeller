package main

import (
	"encoding/json"
	"flag"
	"fmt"
	"html/template"
	"io/ioutil"
	"log"
	"net/http"

	"github.com/gorilla/mux"

	"github.com/FHA-FB5/FortuneTeller/pkg/model"
)

func main() {
	var (
		address    = flag.String("ip", "0.0.0.0", "ip address for server")
		port       = flag.Int("port", 8080, "port for server")
		apiAddress = flag.String("api", "http://0.0.0.0:5000", "api address")
	)
	flag.Parse()
	groupTmpl, err := ioutil.ReadFile("templates/group.gohtml")
	if err != nil {
		log.Fatal(err)
	}
	overviewTmpl, err := ioutil.ReadFile("templates/overview.gohtml")
	if err != nil {
		log.Fatal(err)
	}
	personTmpl, err := ioutil.ReadFile("templates/person.gohtml")
	if err != nil {
		log.Fatal(err)
	}
	groupPage := template.Must(template.New("group").Parse(string(groupTmpl)))
	overviewPage := template.Must(template.New("overview").Parse(string(overviewTmpl)))
	personPage := template.Must(template.New("person").Parse(string(personTmpl)))
	router := mux.NewRouter()
	router.Path("/").HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		response, err := http.Get(*apiAddress + "/groups")
		if err != nil {
			http.Error(w, "can not get groups", http.StatusInternalServerError)
			return
		}
		var groups []*model.Group
		if err := json.NewDecoder(response.Body).Decode(&groups); err != nil {
			http.Error(w, "can not decode response", http.StatusInternalServerError)
			return
		}
		if err := overviewPage.Execute(w, groups); err != nil {
			http.Error(w, "can not execute template", http.StatusInternalServerError)
		}
	})
	router.Path("/{groupID}").HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		groupId, ok := mux.Vars(r)["groupID"]
		if !ok {
			http.Error(w, "no group id supplied", http.StatusBadRequest)
			return
		}
		response, err := http.Get(*apiAddress + "/group/" + groupId)
		if err != nil {
			http.Error(w, "can not get group", http.StatusInternalServerError)
			return
		}
		var group model.Group
		if err := json.NewDecoder(response.Body).Decode(&group); err != nil {
			http.Error(w, "can not decode response body", http.StatusInternalServerError)
			return
		}
		if err := groupPage.Execute(w, group); err != nil {
			http.Error(w, "could not execute template", http.StatusInternalServerError)
			return
		}
	})
	router.Path("/person/{personID}").HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		personID, ok := mux.Vars(r)["personID"]
		if !ok {
			http.Error(w, "no person id supplied", http.StatusBadRequest)
			return
		}
		response, err := http.Get(*apiAddress + "/person/" + personID)
		if err != nil {
			http.Error(w, "could not get person", http.StatusInternalServerError)
			return
		}
		var person model.Person
		if err := json.NewDecoder(response.Body).Decode(&person); err != nil {
			http.Error(w, "could not decode response body", http.StatusInternalServerError)
			return
		}
		response, err = http.Get(*apiAddress + "/group/" + person.GroupID)
		if err != nil {
			http.Error(w, "could not get group", http.StatusInternalServerError)
			return
		}
		var group model.Group
		if err := json.NewDecoder(response.Body).Decode(&group); err != nil {
			http.Error(w, "could not decode response body", http.StatusInternalServerError)
			return
		}
		tmplInput := struct {
			Person model.Person
			Group  model.Group
		}{Person: person, Group: group}
		if err := personPage.Execute(w, tmplInput); err != nil {
			http.Error(w, "could not execute template", http.StatusInternalServerError)
		}
	})
	if err := http.ListenAndServe(fmt.Sprintf("%s:%d", *address, *port), router); err != nil {
		log.Fatal(err)
	}
}
