SELECT survey.sex, IF(survey.birthyear IS NULL, -1, survey.birthyear) AS birthyear, classLk1.name AS lk1, classLk2.name AS lk2, survey.grade, survey.schoolAchivement, survey.sportyCharacter, if( survey.isInRelationship, 'true', 'false' ) AS isInRelationship, if( survey.isAlocoholic, 'true', 'false' ) AS isAlocoholic, if( survey.isSmoker, 'true', 'false' ) AS isSmoker, if( survey.isGamer, 'true', 'false' ) AS isGamer, survey.whyDontPlayGames, survey.viewOfGamers, survey.timeForGamesInSchoolWeek, survey.timeForGamesInHolidayWeek, survey.favoriteGamePlayingSinceMonths, survey.favoriteGameTotalPlaytime, survey.reserveComputerGamesImportant, survey.reserveComputerGamesImpact, survey.reserveComputerGamesHelpStress, survey.reserveComputerGamesLifeBoring, survey.reserveComputerGamesHighlight, survey.reserveComputerGamesParents, survey.reserveComputerGamesSuccess, favoriteGames.names AS favoriteGames, favoriteDrinks.drinks AS favoriteDrinks, favoriteGameGenre.genres AS favoriteGameGenres
FROM `survey`
LEFT JOIN class AS classLk1 ON classLk1.id = survey.lk1
LEFT JOIN class AS classLk2 ON classLk2.id = survey.lk2
LEFT JOIN (SELECT favoriteComputerGame.surveyId, GROUP_CONCAT(computerGame.name) AS names FROM favoriteComputerGame INNER JOIN computerGame ON computerGame.id = favoriteComputerGame.computerGameId GROUP BY favoriteComputerGame.surveyId) AS favoriteGames ON favoriteGames.surveyId = survey.id
LEFT JOIN (SELECT surveyId, GROUP_CONCAT(drink) AS drinks FROM favoriteDrink GROUP BY surveyId) AS favoriteDrinks ON favoriteDrinks.surveyId = survey.id
LEFT JOIN (SELECT surveyId, GROUP_CONCAT(genre) AS genres FROM favoriteGameGenre GROUP BY surveyId) AS favoriteGameGenre ON favoriteGameGenre.surveyId = survey.id