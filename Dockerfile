ARG AGENT_VERSION=1.14.0

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
ARG AGENT_VERSION

RUN apt-get -y update && apt-get -yq install unzip
RUN curl -L -o elastic_apm_profiler_${AGENT_VERSION}.zip https://github.com/elastic/apm-agent-dotnet/releases/download/${AGENT_VERSION}/elastic_apm_profiler_${AGENT_VERSION}-beta.1.zip  
RUN unzip elastic_apm_profiler_${AGENT_VERSION}.zip -d /elastic_apm_profiler_${AGENT_VERSION}

FROM base AS build
ARG AGENT_VERSION

WORKDIR /src
COPY . .

RUN dotnet publish ProfilerExample.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS final
ARG AGENT_VERSION
WORKDIR /app

# The profiler is in a sub directory of the unzipped directory
COPY --from=base /elastic_apm_profiler_${AGENT_VERSION}/elastic_apm_profiler_${AGENT_VERSION}-beta /elastic_apm_profiler
COPY --from=build /app .

ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={FA65FE15-F085-4681-9B20-95E04F6C03CC}
ENV CORECLR_PROFILER_PATH=/elastic_apm_profiler/libelastic_apm_profiler.so
ENV ELASTIC_APM_PROFILER_HOME=/elastic_apm_profiler
ENV ELASTIC_APM_PROFILER_INTEGRATIONS=/elastic_apm_profiler/integrations.yml
ENV ELASTIC_APM_SERVER_URL="http://apm-server:8200"

#ENV ELASTIC_APM_PROFILER_LOG=trace
#ENV ELASTIC_APM_PROFILER_LOG_TARGETS=stdout

ENTRYPOINT ["dotnet", "ProfilerExample.dll"]