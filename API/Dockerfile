FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG PULL_ACCESS_USER
ARG PULL_ACCESS_TOKEN

WORKDIR /src
#RUN rm nuget.config
COPY ["nuget.config.template", "nuget.config"]
RUN sed -i -e "s/USER/$PULL_ACCESS_USER/g" -e "s/PW/$PULL_ACCESS_TOKEN/g" nuget.config
COPY ["API/Resume.API.csproj", "API/"]
RUN dotnet restore "API/Resume.API.csproj"
COPY . .
WORKDIR "./API"
RUN dotnet publish "Resume.API.csproj" -c Release -o /app/publish /p:UseAppHost=false
RUN dotnet nuget remove source Gitlab-BeaniesUtils
RUN dotnet nuget remove source Gitlab-Resume

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 8080
EXPOSE 443
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Resume.API.dll", "--versions=V1", "--health-host=*"]
