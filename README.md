помимо данного проекта нужно скачать проект usertaskmanagementaudit

1. открыть командную строку


2. поднять контейнер с инфраструктурой
    docker compose -f docker-compose.infrastructure.yaml up -d


3. поднять контейнер проекта с юзертасками: 
    (сделал так, что миграции должны выполниться автоматически (для тестирования))
    docker compose -f docker-compose.api.yaml up -d


4. поднять контейнер аудита юзер тасок
   docker compose -f docker-compose.consumer.yaml up -d

    
5. Запустить swagger в браузере http://localhost:8080/swagger/index.html


6. Протестить ручки (userId доступны с 1 по 10 )


7. Перейти в докер и открыть контейнер user_tasks_api, удостовериться, что
   аутбокс и кафка корректно работают и нет ошибок


8. Перейти в докер и открыть контейнер user_tasks_consumer, удостовериться, что 
    логи по событиям пишутся и все работает


9. OpenTelemetry (jaeger) http://localhost:16686


10. Также можно отключить контейнеры user_tasks_api и user_tasks_consumer и потестить,
    запустив локально
    docker compose -f docker-compose.api.yaml down 
    docker compose -f docker-compose.consumer.yaml down
