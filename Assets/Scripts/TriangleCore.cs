using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using TMPro;

public class TriangleCore : MonoBehaviour
{
    [SerializeField]
    private GameObject m_cubePrefab;
    [SerializeField]
    private GameObject m_voidCubePrefab;
    private int m_iteration = 0;
    private List<BigInteger> m_row;
    private Dictionary<(BigInteger, BigInteger), BigInteger> pascalCache = new Dictionary<(BigInteger, BigInteger), BigInteger>();
    [SerializeField]
    private TextMeshPro m_text;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GameObject.FindGameObjectsWithTag("Cube") != null)
            {
                foreach (var item in GameObject.FindGameObjectsWithTag("Cube"))
                {
                    Destroy(item);
                }
            }

            m_row = GenerateRow(m_iteration);
            m_text.text = m_iteration.ToString();

            for (int i = 0; i < m_row.Count; i++)
            {
                string bin = DecimalToBinary(m_row[i]);

                for (int j = 0; j < bin.Length; j++)
                {
                    if (bin[j] == '1')
                    {
                        Instantiate(m_cubePrefab, new UnityEngine.Vector3(i, bin.Length - j, 0), UnityEngine.Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(m_voidCubePrefab, new UnityEngine.Vector3(i, bin.Length - j, 0), UnityEngine.Quaternion.identity);
                    }
                }
            }

            m_iteration++;
        }
    }

    string DecimalToBinary(BigInteger decimalNumber)
    {
        if (decimalNumber == 0)
            return "0";

        string binary = "";

        while (decimalNumber > 0)
        {
            BigInteger remainder = decimalNumber % 2;
            binary = remainder + binary;
            decimalNumber /= 2;
        }

        return binary;
    }
    List<BigInteger> GenerateRow(int rowNumber)
    {
        List<BigInteger> row = new List<BigInteger>();
        for (int j = 0; j <= rowNumber; j++)
        {
            row.Add(CalculatePascalValue(rowNumber, j));
        }
        return row;
    }

    BigInteger CalculatePascalValue(int row, int column)
    {
        // Check if the result is already cached
        if (pascalCache.ContainsKey((row, column)))
        {
            return pascalCache[(row, column)];
        }

        BigInteger result;
        if (column == 0 || column == row)
        {
            result = 1;
        }
        else
        {
            result = CalculatePascalValue(row - 1, column - 1) + CalculatePascalValue(row - 1, column);
        }

        // Cache the result
        pascalCache[(row, column)] = result;

        return result;
    }
}
