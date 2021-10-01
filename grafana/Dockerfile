FROM grafana/grafana:8.1.5

ENV GF_PATHS_PROVISIONING /etc/grafana/provisioning
ENV GF_SERVER_ENABLE_GZIP true 

ADD ./provisioning /etc/grafana/provisioning
ADD ./dashboards /var/lib/grafana/dashboards
