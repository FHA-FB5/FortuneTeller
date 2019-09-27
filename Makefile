EXECUTABLE_NAME=fortune_teller

.PHONY: build deps

pkg/migrations/migrations_gen.go:
	GO111MODULE=off go get github.com/go-bindata/go-bindata/go-bindata
	GO111MODULE=off go generate ./pkg/migrations

deps:
	go mod download

build: deps pkg/migrations/migrations_gen.go
	CGO_ENABLED=0 go build -o ${EXECUTABLE_NAME} ./cmd/server

build_overview: deps
	CGO_ENABLED=0 go build -o overview ./cmd/groups_overview
