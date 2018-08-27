project_path=Gruppenverteilung/Gruppenverteilung/
image_name=einteilungapp
container_name=einteilungapp
volumes=

all:
	@echo "cmd:"
	@echo "  install    creates a docker image of this asp"
	@echo "             project."
	@echo "  run        starts a container and show output"
	@echo "  daemon     starts a container as daemon"
	@echo "  mrproper   removes docker image"

install:
	( \
	pushd "${PWD}/${project_path}"; \
	docker build -t ${image_name} .; \
	popd; \
	)

run:
	docker run --rm -p 5000:80 ${volumes} ${container_name} ${image_name}

daemon:
	docker run -d --rm -p 5000:80 ${volumes} ${container_name} ${image_name}


mrproper:
	docker rmi ${image_name}

