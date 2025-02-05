<?php

namespace App\Services;

use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Log;

class JawalySMSService
{
    protected $baseUrl;
    protected $appId;
    protected $appSecret;

    public function __construct()
    {
        $this->baseUrl = config('services.jawaly.base_url');
        $this->appId = config('services.jawaly.app_id');
        $this->appSecret = config('services.jawaly.app_secret');
    }

    protected function getAuthHeader()
    {
        return 'Basic ' . base64_encode($this->appId . ':' . $this->appSecret);
    }

    public function sendSMS(string $message, array $numbers, string $sender)
    {
        try {
            $response = Http::withHeaders([
                'Accept' => 'application/json',
                'Content-Type' => 'application/json',
                'Authorization' => $this->getAuthHeader(),
            ])->post($this->baseUrl . 'account/area/sms/send', [
                'messages' => [
                    [
                        'text' => $message,
                        'numbers' => $numbers,
                        'sender' => $sender
                    ]
                ]
            ]);

            return $this->handleResponse($response);
        } catch (\Exception $e) {
            Log::error('Error sending SMS: ' . $e->getMessage());
            return ['success' => false, 'message' => 'Error sending SMS'];
        }
    }

    public function checkBalance()
    {
        try {
            $response = Http::withHeaders([
                'Accept' => 'application/json',
                'Content-Type' => 'application/json',
                'Authorization' => $this->getAuthHeader(),
            ])->get($this->baseUrl . 'account/area/me/packages');

            if ($response->successful()) {
                $data = $response->json();
                if ($data['code'] === 200) {
                    return [
                        'success' => true,
                        'balance' => $data['total_balance']
                    ];
                }
            }

            return ['success' => false, 'message' => $response->json()['message'] ?? 'Error checking balance'];
        } catch (\Exception $e) {
            Log::error('Error checking balance: ' . $e->getMessage());
            return ['success' => false, 'message' => 'Error checking balance'];
        }
    }

    public function getSenders()
    {
        try {
            $response = Http::withHeaders([
                'Accept' => 'application/json',
                'Content-Type' => 'application/json',
                'Authorization' => $this->getAuthHeader(),
            ])->get($this->baseUrl . 'account/area/senders');

            if ($response->successful()) {
                $data = $response->json();
                if ($data['code'] === 200) {
                    return [
                        'success' => true,
                        'senders' => collect($data['items'])->pluck('sender_name')
                    ];
                }
            }

            return ['success' => false, 'message' => $response->json()['message'] ?? 'Error fetching senders'];
        } catch (\Exception $e) {
            Log::error('Error fetching senders: ' . $e->getMessage());
            return ['success' => false, 'message' => 'Error fetching senders'];
        }
    }

    protected function handleResponse($response)
    {
        if ($response->successful()) {
            $data = $response->json();
            if (isset($data['job_id'])) {
                return [
                    'success' => true,
                    'message' => 'Message sent successfully',
                    'job_id' => $data['job_id']
                ];
            }
            
            if (isset($data['messages'][0]['err_text'])) {
                return [
                    'success' => false,
                    'message' => $data['messages'][0]['err_text']
                ];
            }
        }

        if ($response->status() === 422) {
            return ['success' => false, 'message' => 'Empty message text'];
        }

        return [
            'success' => false,
            'message' => $response->json()['message'] ?? 'Unknown error occurred'
        ];
    }
}
