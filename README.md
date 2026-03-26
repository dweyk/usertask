помимо данного проекта нужно скачать проект usertaskmanagementaudit и установить ПО (см. самый низ readme)

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

УСТАНОВКА ПО

1. Проверка виртуализации:
Открой Диспетчер задач (Ctrl + Shift + Esc) -> вкладка Производительность -> ЦП.
Справа внизу должно быть написано: Виртуализация: Включена.
⚠️ Если выключена: Нужно зайти в BIOS/UEFI при загрузке ПК и включить Intel VT-x или AMD-V.

2. Установка WSL2:
Открой PowerShell от имени Администратора (Правый клик на Пуск -> Терминал (Администратор)) и выполни:
wsl --install

3. Настройка WSL2:
После перезагрузки открой приложение Ubuntu из меню Пуск. Тебя попросят создать username и password для Linux-среды. Запомни их (они не отображаются при вводе).
Вернись в PowerShell (Администратор) и убедись, что версия 2:
wsl --set-default-version 2

4. установить docker desktop
Скачай версию для Windows (WSL 2 backend).
  4.1. Первичный запуск:
	Запусти Docker Desktop.
	Прими условия лицензии.
	В настройках (шестеренка) -> General: Убедись, что стоит галочка "Use the WSL 2 based engine".
	В настройках -> Resources -> WSL Integration: Убедись, что активирована интеграция с твоим дистрибутивом Ubuntu.

  4.2. Убедись, что отсутствует галочка на: Expose daemon on tcp://localhost:2375 without TLS
