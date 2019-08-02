FROM golang AS builder

COPY . /FortuneTeller
WORKDIR /FortuneTeller
RUN go mod download
RUN CGO_ENABLED=0 go build -o /fortune_teller /FortuneTeller/cmd/server

FROM scratch
COPY --from=builder /fortune_teller /fortune_teller
EXPOSE 5000/tcp
ENTRYPOINT ["/fortune_teller"]