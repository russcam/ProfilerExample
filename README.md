# Profiler example

1. clone this repository
2. clone https://github.com/elastic/apm-integration-testing
3. Start docker 7.16.0 Elastic stack from apm-integration-testing directory

   ```sh
   python .\scripts\compose.py start 7.16.0
   ```
4. Start mysql docker container on the same docker network

    ```sh
    docker run --name some-mysql -e MYSQL_ROOT_PASSWORD=my-secret-pw -e MYSQL_USER=dbuser -e MYSQL_PASSWORD=my-secret-pw -e MYSQL_DATABASE=test -p 3306:3306 --name mysql --network apm-integration-testing -d mysql
    ```

5. Build this application from this repository local directory

    ```
    docker build -t profilerexample .
    ```
6. Run application on same docker network

    ```
    docker run -p 5000:80 profilerexample
    ```
7. Open app on http://localhost:5000. Observe that there are MySql version details on the landing page.
8. Open Kibana at http://localhost:5601 and log in with `admin` and password `changeme`
9. Navigate to the APM UI and observe that the transaction contains MySql spans.