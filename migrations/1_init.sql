-- +migrate Up

create table groups
(
    id         uuid      not null primary key,
    created_at timestamp not null check ( created_at <= now() ),
    updated_at timestamp not null check ( updated_at <= now() ) default now(),
    deleted_at timestamp null check ( deleted_at <= now() ),
    name       text      not null
);

create table persons
(
    id         uuid      not null primary key,
    created_at timestamp not null check ( created_at <= now() ),
    updated_at timestamp not null check ( updated_at <= now() ) default now(),
    deleted_at timestamp null check ( deleted_at <= now() ),
    group_id   uuid      not null references groups,
    surname    text      not null,
    name       text      not null,
    major      text      not null,
    gender     text      not null,
    age        integer   not null check ( age > 0 )
);

-- +migrate Down

drop table persons;
drop table groups;
