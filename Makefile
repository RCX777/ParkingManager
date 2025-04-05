ifeq ($(shell uname), Darwin)
	CONTAINER_ENGINE := podman
else
	CONTAINER_ENGINE := docker
endif

COMPOSE_FILE = $(PWD)/docker/compose.yml

all: run

run: stop
	$(CONTAINER_ENGINE) compose -f $(COMPOSE_FILE) up --build

stop:
	$(CONTAINER_ENGINE) compose -f $(COMPOSE_FILE) down

clean: stop
	rm -rf \
		.db_data \
		backend/bin backend/obj \
		backend/Migrations \
		backend/TempDocuments

