{{/*
Expand the name of the chart.
*/}}
{{- define "brand-gamecredits.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "brand-gamecredits.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "brand-gamecredits.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "brand-gamecredits.labels" -}}
helm.sh/chart: {{ include "brand-gamecredits.chart" . }}
{{ include "brand-gamecredits.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "brand-gamecredits.selectorLabels" -}}
app.kubernetes.io/name: {{ include "brand-gamecredits.name" . }}
app: {{ include "brand-gamecredits.name" . }}
tgs.kind: brand
tgs.brand.kind: gamecredits
tgs.gamecredits.kind: gamecredits
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "brand-gamecredits.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "brand-gamecredits.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}
