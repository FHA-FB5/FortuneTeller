# FortuneTeller

FortuneTeller is a group assignment service written in Go. Its purpose is accepting
entries from people with some information (gender, age, major) and assigning them to
a group. It will balance the people assigned to each group, so that there is an even
distribution based on the provided information.

## Building

To build FortuneTeller clone this repository and run the `docker-compose.yml`

```bash
git clone https://github.com/FHA-FB5/FortuneTeller
cd FortuneTeller
docker-compose up -d
```

By default the api server is running on port 5000. This can be changed by adjusting
the `docker-compose.yml`.

## Documentation

The API documentation can be found at https://docs.fsr5.de/

## Contributing

General guidelines for contributing to this project can be found at the
[docs repo](https://github.com/FHA-FB5/docs/blob/master/README.md)
