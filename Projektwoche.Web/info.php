<?php
require_once 'config.php';

/**
 * Created by PhpStorm.
 * User: anaph
 * Date: 26.01.2018
 * Time: 19:08
 */

$mysqli = new mysqli(MYSQL_HOST, MYSQL_USERNAME, MYSQL_PASSWORD, MYSQL_DATABASENAME);
if ($mysqli->connect_errno) {
    http_response_code(500);
    print json_encode([
        'errorId' => 3,
        'errorMessage' => 'Error when connecting to database: ' . $mysqli->connect_error
    ]);
    exit;
}

$computerGames = [];
$classes = [];

$result = $mysqli->query('SELECT id, `name` FROM computerGame');
while ($row = $result->fetch_assoc()) {
    $computerGames[] = [
        'id' => (int) $row['id'],
        'name' => utf8_encode($row['name'])
    ];
}
$result->free();

$result = $mysqli->query('SELECT id, `name` FROM class');
while ($row = $result->fetch_assoc()) {
    $classes[] = [
        'id' => (int) $row['id'],
        'name' => utf8_encode($row['name'])
    ];
}
$result->free();

echo json_encode([
    'computerGames' => $computerGames,
    'classes' => $classes
]);