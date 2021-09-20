using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PokemonManager : MonoBehaviour
{

	private TileManager tileManager;
	
	[SerializeField]
	private float waitSpawnTime, minIntervalTime, maxIntervalTime;

	private List<Pokemon> pokemons = new List<Pokemon>();

	[SerializeField]
	private Text scoreText;

	void Start()
	{
		tileManager = GameObject.FindGameObjectWithTag("TileManager").GetComponent<TileManager>();
		if (!PlayerPrefs.HasKey("SCORE")) {
			PlayerPrefs.SetInt("SCORE", 0);
		}
	}

	void Update()
	{
		if (waitSpawnTime < Time.time) {
			waitSpawnTime = Time.time + UnityEngine.Random.Range(minIntervalTime, maxIntervalTime);
			SpawnPokemon();
		}

		int currScore = PlayerPrefs.GetInt("SCORE");
		print("score from pokemanager: " + currScore);
		scoreText.text = "Player Score: " + currScore;

		if (Input.touchCount == 0) return;

		Touch touch = Input.GetTouch(0);

		if (Input.touchCount == 1 && touch.phase == TouchPhase.Stationary) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(touch.position);

			if (Physics.Raycast(ray, out hit, 1000f)) {
				if (hit.transform.tag == "Pokemon") {
					// Pokemon pokemon = hit.transform.GetComponent<Pokemon>();
					PokemonBattle();
				}
			}
		}
	}

	void PokemonBattle()
	{
		SceneManager.LoadScene("CatchScene");
	}

	void SpawnPokemon()
	{
		PokemonType type = (PokemonType)(int)UnityEngine.Random.Range(0, Enum.GetValues(typeof(PokemonType)).Length);
		float newLat = UnityEngine.Random.Range(-50, 50);
		float newLon = UnityEngine.Random.Range(-50, 50);

		Pokemon prefab = Resources.Load("MapPokemon/PikachuF", typeof(Pokemon)) as Pokemon;
		Pokemon pokemon = Instantiate(prefab, Vector3.zero, Quaternion.identity) as Pokemon;

		pokemon.Init(newLat, newLon);
		pokemons.Add(pokemon);
	}

	public void UpdatePokemonPosition()
	{
		if (pokemons.Count == 0) return;

		Pokemon[] pokemon = pokemons.ToArray();
		for (int i = 0; i < pokemon.Length; i++) {
			pokemon[i].UpdatePosition();
		}
	}
}