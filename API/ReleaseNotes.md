# VERSION 1.0.0
Initial full release of API

# VERSION 1.0.0-rc-1
Skipped straight to release 1.0.0

# VERSION 1.0.0-beta-1
Skipped straight to release 1.0.0

# VERSION 1.0.0-alpha.beta
Skipped straight to release 1.0.0

# Version 1.0.0-alpha-1.1.1
- Change cli verbose command to take a range of options to set log level
- Move Serilog service registration to be handled by application

# Version 1.0.0-alpha-1.1.0
- Add Test endpoint
- Updated Models
- Update endpoint headers
- Remove endpoints that should be reached
- Add new Models
- Change EducationInstitution to Organization
- Correct Update Endpoint behavior to just override the existing model with the
	data in the update request
- Generalize static endpoint information into a Contracts class to be used in SDK
- Add pipeline
- Add k8s yaml config file
- Add dockerfile and dockerignore
- Add command line interface actions
- Cleaned up warnings
- Update Dependencies

# Version 1.0.0-alpha-1.0.0
- Implmented Basic CRUD operations
- Implemented logging and get filtering

# VERSION 0.1.0-alpha
- This is the initial prerelease build as I work towards a version that can be
	used as a templete to quickly generate any other API I might need

## OVERVIEW
- Backend service functions
- Endpoints following the REST Api standards
- Basic logging framework
- XML documentation