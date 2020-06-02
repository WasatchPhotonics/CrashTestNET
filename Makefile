help:
	@echo "This app is built using Visual Studio; see README.md"

.PHONY: clean 

clean:
	@rm -rf src/{bin,obj}                   \
            Setup{32,64}/{Debug,Release}    \
            .vs
