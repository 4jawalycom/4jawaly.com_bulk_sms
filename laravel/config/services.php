<?php

return [
    // ... other services configs

    'jawaly' => [
        'base_url' => env('JAWALY_BASE_URL', 'https://api-sms.4jawaly.com/api/v1/'),
        'app_id' => env('JAWALY_APP_ID'),
        'app_secret' => env('JAWALY_APP_SECRET'),
    ],
];
