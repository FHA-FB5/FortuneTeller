FROM golang AS builder

COPY . /FortuneTeller
WORKDIR /FortuneTeller
RUN make build

FROM scratch
COPY --from=builder /FortuneTeller/fortune_teller /fortune_teller
EXPOSE 5000/tcp
ENTRYPOINT ["/fortune_teller"]
