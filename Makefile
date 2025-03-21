ifeq ($(shell uname), Darwin)
	CONTAINER_ENGINE := podman
else
	CONTAINER_ENGINE := docker
endif

COMPOSE_FILE = $(PWD)/docker/compose.yml

all: run

run:
	$(CONTAINER_ENGINE) compose -f $(COMPOSE_FILE) up -d --build

stop:
	$(CONTAINER_ENGINE) compose -f $(COMPOSE_FILE) down

