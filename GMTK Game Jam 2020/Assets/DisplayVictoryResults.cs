using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayVictoryResults : MonoBehaviour
{
    public static int playerScore;

    public float scoreDelay;

    public GameObject thanksForPlaying;
    public GameObject[] scoreBags;
    public GameObject spotlight;

    [SerializeField]
    private Result[] presetScores;

    private Result[] scores;

    [System.Serializable]
    private class Result {
        public bool isPlayer;
        public string name;
        public string title;
        public int score;

        public Result(bool pIsPlayer, string pName, string pTitle, int pScore)
        {
            isPlayer = pIsPlayer;
            name = pName;
            title = pTitle;
            score = pScore;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Generate scores
        scores = new Result[4];

        bool playerPlaced = false;
        int scoreIndex = 0;
        for (int presetScoreIndex=0; presetScoreIndex < presetScores.Length; presetScoreIndex++)
        {
            if (!playerPlaced && (playerScore > presetScores[presetScoreIndex].score))
            {
                scores[scoreIndex] = new Result(true, "You", "The Player", playerScore);
                scoreIndex++;
                playerPlaced = true;
            }

            scores[scoreIndex] = presetScores[presetScoreIndex];
            scoreIndex++;
        }

        if (!playerPlaced)
        {
            scores[scoreIndex] = new Result(true, "You", "The Player", playerScore);
        }

        StartCoroutine(ShowResults());
    }

    IEnumerator ShowResults()
    {
        for (int i=0; i<scoreBags.Length; i++)
        {
            yield return new WaitForSeconds(scoreDelay);
            GameObject currentScoreBag = scoreBags[i];
            Result currentResult = scores[i];

            if (currentResult.isPlayer)
            {
                spotlight.transform.position = new Vector2(currentScoreBag.transform.position.x, spotlight.transform.position.y);
                spotlight.SetActive(true);
            }

            currentScoreBag.GetComponent<ScoreBag>().Initialize(currentResult.name, currentResult.title, currentResult.score);
            currentScoreBag.SetActive(true);
        }

        yield return new WaitForSeconds(scoreDelay/2);
        thanksForPlaying.SetActive(true);
    }
}
