using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        public static GameManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        #endregion


        [SerializeField] private Planet _planetPrefab;

        [SerializeField] private Sprite[] _planetSprites;

        public readonly float Step = 100f;

        [SerializeField] private float _planetSpawnChance = 0.3f;

        [SerializeField] private int _minRank = 1;
        [SerializeField] private int _maxRank = 10000;

        private const float _perlinScale = 1.5551f;
        private const float _perlinOffset = 100000.0f;

        private Camera _cam;

        private List<Planet> _planetsPool = new List<Planet>();
        private HashSet<Vector2Int> _usedCoords = new HashSet<Vector2Int>();

        public void Start()
        {
            _cam = Camera.main;
            OnCameraMoved();
        }

        public bool SpawnPlanet(Vector2Int pos, int rank)
        {
            if (_usedCoords.Contains(pos)) return false;

            Planet newPlanet = null;
            // if Pool has unused planets
            foreach (var planet in _planetsPool)
            {
                if (planet.Hidden)
                {
                    newPlanet = planet;
                    newPlanet.ShowPlanet();
                    break;
                }
            }

            if (newPlanet == null)
            {
                newPlanet = Instantiate(_planetPrefab, transform);
                _planetsPool.Add(newPlanet);
            }

            newPlanet.transform.position = new Vector2(pos.x, pos.y) * Step;

            int seed = newPlanet.GetHashCode();
            Random.InitState(seed);

            int n = rank % _planetSprites.Length;
            if (n < 0 || n >= _planetSprites.Length)
            {

            }
            Sprite randomSprite = _planetSprites[n];

            newPlanet.Draw(randomSprite, rank);

            _usedCoords.Add(pos);

            return true;
        }

        private Vector2 SnapToGrid(Vector2 pos)
        {
            pos.x = Mathf.Ceil(pos.x / Step) * Step;
            pos.y = Mathf.Ceil(pos.y / Step) * Step;
            return pos;
        }

        public void OnCameraMoved()
        {
            DeleteUnusedPlanets();

            Vector3 topLeft = _cam.ViewportToWorldPoint(new Vector3(0f, 1f, _cam.nearClipPlane));
            Vector3 botRight = _cam.ViewportToWorldPoint(new Vector3(1f, 0f, _cam.nearClipPlane));

            Vector2Int topLeftInt = new Vector2Int((int)(topLeft.x / Step), (int)(topLeft.y / Step));
            Vector2Int botRightInt = new Vector2Int((int)(botRight.x / Step), (int)(botRight.y / Step));

            for (int x = topLeftInt.x-1; x <= botRightInt.x; x ++)
            {
                for (int y = topLeftInt.y+1; y >= botRightInt.y; y--)
                {
                    float perlinValue = Mathf.PerlinNoise(x * _perlinScale + _perlinOffset,
                        y * _perlinScale + _perlinOffset);

                    if (perlinValue <= _planetSpawnChance)
                    {
                        // map [0f; 0.3f] to [1; 10000]
                        int rank = (int)(perlinValue * (_maxRank - _minRank) / _planetSpawnChance + _minRank);
                        rank = Math.Abs(rank);

                        SpawnPlanet(new Vector2Int(x, y), rank);
                    }
                }
            }
        }

        private void DeleteUnusedPlanets()
        {
            Camera cam = Camera.main;
            foreach (var planet in _planetsPool)
            {
                if (planet.Hidden) continue;

                var rect = planet.transform as RectTransform;
                if (!rect.IsVisibleFrom(cam))
                {
                    planet.HidePlanet();
                    Vector2Int intPos = new Vector2Int((int) (planet.transform.position.x / Step),
                        (int) (planet.transform.position.y / Step));
                    _usedCoords.Remove(intPos);
                }
            }
        }

        public void OnShipMoved(Vector2 newPos)
        {
            foreach (var p in _planetsPool)
            {
                p.HideRank();
            }

            // order by closest to ship
            var closestTen = (from planet in _planetsPool 
                orderby Vector2.Distance(planet.transform.position, newPos)
                select planet).Take(10);
            foreach (var planet in closestTen)
            {
                planet.DisplayRank();
            }

        }
    }
}
