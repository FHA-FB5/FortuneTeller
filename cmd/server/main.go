package main

import (
	"context"
	"flag"
	"fmt"
	"net/http"
	"time"

	"github.com/gorilla/mux"
	"github.com/jmoiron/sqlx"
	_ "github.com/lib/pq"
	migrate "github.com/rubenv/sql-migrate"
	log "github.com/sirupsen/logrus"

	"github.com/FHA-FB5/FortuneTeller/pkg/api"
	"github.com/FHA-FB5/FortuneTeller/pkg/assignment"
	"github.com/FHA-FB5/FortuneTeller/pkg/database"
	"github.com/FHA-FB5/FortuneTeller/pkg/migrations"
)

func main() {
	var ipAddress, dbAddress string
	var port, dbRetries, dbRetryWaitTime int
	flag.StringVar(&ipAddress, "ip", "0.0.0.0", "ip address to listen on")
	flag.IntVar(&port, "port", 5000, "port to listen on")
	flag.StringVar(&dbAddress, "db", "postgres://postgres@postgres:5432?sslmode=disable",
		"address of the postgres database")
	flag.IntVar(&dbRetries, "db.retries", 5,
		"number of times the server should try to connect to the database before aborting")
	flag.IntVar(&dbRetryWaitTime, "db.retrywait", 5, "time to wait between retries")
	flag.Parse()
	ctx := context.Background()
	logger := log.StandardLogger()
	var db *sqlx.DB
	var err error
	for i := 0; i < dbRetries; i++ {
		db, err = sqlx.Connect("postgres", dbAddress)
		if err != nil {
			log.Warnf("postgres db is not ready, waiting %d seconds: %v", dbRetryWaitTime, err)
			time.Sleep(time.Duration(dbRetryWaitTime) * time.Second)
		}
	}
	if err != nil {
		log.Fatal(err)
	}
	if _, err := migrate.Exec(db.DB, "postgres", &migrate.AssetMigrationSource{
		Asset:    migrations.Asset,
		AssetDir: migrations.AssetDir,
		Dir:      "../../migrations",
	}, migrate.Up); err != nil {
		log.Fatal(err)
	}
	personRepo, err := database.NewPersonRepository(ctx, db)
	if err != nil {
		log.Fatal(err)
	}
	groupRepo, err := database.NewGroupRepository(ctx, db)
	if err != nil {
		log.Fatal(err)
	}
	groups, err := groupRepo.List(ctx)
	if err != nil {
		logger.Fatal(err)
	}
	for i, group := range groups {
		if group == nil {
			logger.Fatal("groups should not be nil")
		}
		persons, err := personRepo.ListByGroup(ctx, group.ID)
		if err != nil {
			log.Fatal(err)
		}
		groups[i].Members = persons
	}
	assigner := assignment.NewAssigner(groups)
	personService, err := api.NewPersonService(personRepo, assigner, logger)
	if err != nil {
		log.Fatal(err)
	}
	groupService, err := api.NewGroupService(personRepo, groupRepo, assigner, logger)
	if err != nil {
		log.Fatal(err)
	}
	router := mux.NewRouter()
	router.Use(func(h http.Handler) http.Handler {
		return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
			w.Header().Add("Access-Control-Allow-Origin", "*")
			w.Header().Add("Content-Type", "application/json")
			h.ServeHTTP(w, r)
		})
	})
	router.HandleFunc("/groups", groupService.List).Methods(http.MethodGet)
	router.HandleFunc("/group/{id}", groupService.Get).Methods(http.MethodGet)
	router.HandleFunc("/group/{id}", groupService.Update).Methods(http.MethodPut)
	router.HandleFunc("/group", groupService.Create).Methods(http.MethodPost)
	router.HandleFunc("/person/{id}", personService.Get).Methods(http.MethodGet)
	router.HandleFunc("/person/{id}", personService.Update).Methods(http.MethodPut)
	router.HandleFunc("/person", personService.Create).Methods(http.MethodPost)
	log.Infof("Listening on %s:%d", ipAddress, port)
	if err := http.ListenAndServe(fmt.Sprintf("%s:%d", ipAddress, port), router); err != nil {
		log.Error(err)
	}
}
