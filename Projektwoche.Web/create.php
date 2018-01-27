<?php
require_once 'config.php';
/**
 * Created by PhpStorm.
 * User: anaph
 * Date: 27.01.2018
 * Time: 15:30
 */


$header = $_SERVER['HTTP_CUSTOMAUTHORIZATION'];
if ($header !== SECRETKEY)
{
    http_response_code(401);
    exit;
}

$mysqli = new mysqli(MYSQL_HOST, MYSQL_USERNAME, MYSQL_PASSWORD, MYSQL_DATABASENAME);
if ($mysqli->connect_errno) {
    http_response_code(500);
    print json_encode([
        'errorId' => 3,
        'errorMessage' => 'Error when connecting to database: ' . $mysqli->connect_error
    ]);
    exit;
}

function ExecuteStatment($statement) {
    if(!$statement->execute())
    {
        http_response_code(500);
        print json_encode([
            'errorId' => 4,
            'errorMessage' => 'Error executing mysql insert.'
        ]);
        exit;
    }
}

$data = json_decode(file_get_contents('php://input'), false);

$sex = $data->isMale === true ? 'm' : 'f';
$birthyear = $data->birthyear;
$lk1 = $data->lk1 == null ? null : $data->lk1->id;
$lk2 = $data->lk2 == null ? null : $data->lk2->id;
$schoolAchivement = trim($data->schoolAchivement, 'a');

$entryId = 0;
if ($data->isGamer === true) {
    $sql = <<<'SQL'
 INSERT INTO `survey`
            (`sex`,
             `birthyear`,
             `lk1`,
             `lk2`,
             `grade`,
             `schoolachivement`,
             `sportycharacter`,
             `isinrelationship`,
             `isalocoholic`,
             `issmoker`,
             `isgamer`,
             `timeforgamesinschoolweek`,
             `timeforgamesinholidayweek`,
             `favoritegameplayingsincemonths`,
             `favoritegametotalplaytime`,
             `reservecomputergamesimportant`,
             `reservecomputergamesimpact`,
             `reservecomputergameshelpstress`,
             `reservecomputergameslifeboring`,
             `reservecomputergameshighlight`,
             `reservecomputergamesparents`,
             `reservecomputergamessuccess`)
VALUES      (?,
             ?,
             ?,
             ?,
             ?,
             ?,
             ?,
             ?,
             ?,
             ?,
             true,
             ?,
             ?,
             ?,
             ?,
             ?,
             ?,
             ?,
             ?,
             ?,
             ?,
             ?)
SQL;
    $stmt = $mysqli->prepare($sql);
    $stmt->bind_param('siiisssiiiiiiiiiiiiii',$sex, $data->birthyear, $lk1, $lk2, $data->schoolGrade, $schoolAchivement, $data->sportyCharacter, $data->isInRelationship,
        $data->isAlcoholic, $data->isSmoker, $data->timeForComputerGamesSchoolWeek, $data->timeForComputerGamesHolidayWeek, $data->favoriteGamePlayingSince, $data->favoriteGameTotalPlaytime,
        $data->reserveComputerGamesImportant, $data->reserveComputerGamesImpact, $data->reserveComputerGamesHelpStress, $data->reserveComputerGamesLifeBoring,
        $data->reserveComputerGamesHighlight, $data->reserveComputerGamesParents, $data->reserveComputerGamesSuccess);

    ExecuteStatment($stmt);
    $stmt->close();
    $entryId = $mysqli->insert_id;
} else {
    $sql = <<<'SQL'
 INSERT INTO `survey`
            (`sex`,
             `birthyear`,
             `lk1`,
             `lk2`,
             `grade`,
             `schoolachivement`,
             `sportycharacter`,
             `isinrelationship`,
             `isalocoholic`,
             `issmoker`,
             `isgamer`,
             `whyDontPlayGames`,
             `viewOfGamers`)
VALUES      (?,
             ?,
             ?,
             ?,
             ?,
             ?,
             ?,
             ?,
             ?,
             ?,
             false,
             ?,
             ?);  
SQL;

    $stmt = $mysqli->prepare($sql);
    $stmt->bind_param('siiisssiiiss',$sex, $data->birthyear, $lk1, $lk2, $data->schoolGrade, $schoolAchivement, $data->sportyCharacter, $data->isInRelationship,
        $data->isAlcoholic, $data->isSmoker, $data->reasonForNotPlaying, $data->viewOfGamers);

    ExecuteStatment($stmt);
    $stmt->close();
    $entryId = $mysqli->insert_id;
}

$stmt = $mysqli->prepare('INSERT INTO favoriteDrink (surveyId, drink) VALUES (?, ?)');
$favoriteDrink = '';
$stmt->bind_param('is', $entryId, $favoriteDrink);
foreach ($data->favoriteDrinks as $favoriteDrink) {
    ExecuteStatment($stmt);
}
$stmt->close();

if ($data->isGamer === true) {
    if ($data->favoriteGames != null) {
        $stmt = $mysqli->prepare('INSERT INTO favoriteComputerGame (surveyId, computerGameId) VALUES (?, ?)');
        $favoriteGame = 0;
        $stmt->bind_param('ii', $entryId, $favoriteGame);
        foreach ($data->favoriteGames as $computerGame) {
            $favoriteGame = $computerGame->id;
            ExecuteStatment($stmt);
        }
        $stmt->close();
    }

    if ($data->favoriteGameGenre != null) {
        $stmt = $mysqli->prepare('INSERT INTO favoriteGameGenre (surveyId, genre) VALUES (?, ?)');
        $favoriteGenre = '';
        $stmt->bind_param('is', $entryId, $favoriteGenre);
        foreach ($data->favoriteGameGenre as $favoriteGenre) {
            ExecuteStatment($stmt);
        }
        $stmt->close();
    }

    if ($data->gamingPlatforms != null) {
        $stmt = $mysqli->prepare('INSERT INTO primaryGamingPlatform (surveyId, platform) VALUES (?, ?)');
        $gamingPlatform = '';
        $stmt->bind_param('is', $entryId, $gamingPlatform);
        foreach ($data->gamingPlatforms as $gamingPlatform) {
            ExecuteStatment($stmt);
        }
        $stmt->close();
    }
}

http_response_code(201);
print $entryId;
exit;