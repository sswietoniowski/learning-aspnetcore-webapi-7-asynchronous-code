import http from 'k6/http';

const baseUrl = 'http://localhost:5000';
const bookId = 'c0a80121-48aa-48b0-8c0c-927b1f8eac2c';
const mode = 'sync'; // 'async'

export default function () {
  http.get(`${baseUrl}/api/books/${mode}/${bookId}`);
}

// To run this test, use the following command from the root of the repository:
// k6 run --vus 8 --duration 60s books\api\Tests\Scripts\basic-test.js
