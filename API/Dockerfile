ARG PULL_ACCESS_TOKEN

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["API/Resume.API.csproj", "API/"]
RUN dotnet nuget add source "https://gitlab.com/api/v4/groups/100168093/-/packages/nuget/index.json" -n Gitlab-BeaniesUtils -u beanieroxiicazier -p PULL_ACCESS_TOKEN --store-password-in-clear-text
RUN dotnet nuget add source "https://gitlab.com/api/v4/groups/99246157/-/packages/nuget/index.json" -n Gitlab-Resume -u beanieroxiicazier -p PULL_ACCESS_TOKEN --store-password-in-clear-text
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
