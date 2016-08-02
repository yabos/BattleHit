using UnityEngine;
//using System;
using System.Collections;
using System.Collections.Generic;
//using System.IO;

public class StringTable
{
	protected Dictionary<string, List<string> > m_Elem = null;
	protected List<string> m_ColumnIndex = null;
	
	public int row
	{
		get
		{
			if(m_Elem == null || m_Elem.Count == 0) { return 0; }
			string label = GetLabelNameByColumn(0);
			List<string> col = null;
			if(string.IsNullOrEmpty(label) == true || m_Elem.TryGetValue(label, out col) == false) { return 0; }
			return col.Count;
		}
	}
	
	public int column
	{
		get { return m_ColumnIndex.Count; }
	}
	
	public StringTable()
	{
		m_Elem = new Dictionary<string, List<string> >();
		m_ColumnIndex = new List<string>();
	}
	
	public string GetLabelNameByColumn(int iCol)
	{
		return m_ColumnIndex[iCol];
	}
	
	protected void PushLabel(string strLabelName)
	{
		if(string.IsNullOrEmpty(strLabelName) == true)
		{
			Debug.LogError("strLabelName: Label name is empty!");
			return;
		}
		if(m_Elem.ContainsKey(strLabelName) == true)
		{ //Is already exist label? Then return.
			Debug.LogError("StringTable: Same label name is already exist!!! [" + strLabelName + "]");
			return;
		}
		
		m_ColumnIndex.Add(strLabelName);
		m_Elem.Add(strLabelName, new List<string>());
	}
	
	protected bool PushValue(int iCol, string strValue)
	{
		string strLabelName = GetLabelNameByColumn(iCol);
		if(string.IsNullOrEmpty(strLabelName) == false && m_ColumnIndex.Count > 0)
		{
			List<string> Row = null;
			if(m_Elem.TryGetValue(strLabelName, out Row))
			{
				Row.Add(strValue);
			}
			else
			{
				Debug.LogError("StringTable: Detected missmatch data! row: " + this.row + "(" + strLabelName + "), column: " + iCol);
				return false;
			}
		}
		
		return true;		
	}
	
	public string GetValue(int iRow, string strLabelName)
	{
		if(string.IsNullOrEmpty(strLabelName) == true) { return null; }
		
		List<string> Row = null;
		if(m_Elem.TryGetValue(strLabelName, out Row) == false)
		{
			Debug.LogError("StringTable: Cannot find " + strLabelName);
			return null;
		}
		if(Row == null || iRow >= Row.Count) { return null; }
		return Row[iRow];
	}
	
	public int GetValueAsInt(int iRow, string strLabelName)
	{
		string strRet = GetValue(iRow, strLabelName);
		if(string.IsNullOrEmpty(strRet) == true) { return -1; }
		int iVal;
		if(int.TryParse(strRet, out iVal) == false) { return -1; }
		return iVal;
	}
	
	public bool Build(TextAsset textAsset)
	{
		if( textAsset == null ) { return false; }
		
		try
		{
			if( string.IsNullOrEmpty( textAsset.text ) )
			{
				Debug.LogError("StringTable: Build file data is empty!");
				return false;
			}
			
			string [] lines = textAsset.text.Split( "\r\n".ToCharArray() );
			
			int row = 0, col = 0;
			
			string [] prams = lines[0].Split( ",".ToCharArray() );

			for( int i=0; i < lines.Length; ++i )
			{
				if( string.IsNullOrEmpty( lines[i] ) ) { continue; }
				
				string [] tokens = lines[i].Split( ",".ToCharArray() );
				
				for( int j=0; j < prams.Length; ++j )
				{
					if( string.IsNullOrEmpty( tokens[j] ) )
					{
						tokens[j] = "";
					}
					
					//if( string.IsNullOrEmpty( tokens[j] ) == false )
					{
						tokens[j] = tokens[j].Trim( " \t".ToCharArray() );
						
						if( row == 0 )
						{
							PushLabel( tokens[j] );
						}
						else
						{
							if( PushValue( col, tokens[j] ) == false )
							{
								return false;
							}
						}
						
						
						++col;
					}
//					else
//					{
//						Debug.LogError( "Token is null or empty" );
//					}
				}
				
				col = 0;
				++row;
			}
			
		}
		catch(System.Exception e)
		{
			Debug.LogError("StringTable: " + e.Message);
			return false;
		}
		
		return true;
	}
	
	public bool Build(string strPath)
	{
		if(string.IsNullOrEmpty(strPath))
		{
			Debug.LogError("StringTable: Build file path is empty!");
			return false;
		}
		
		try
		{
			TextAsset textAsset = VResources.Load<TextAsset>(strPath);
			
			if( textAsset == null )
			{
				Debug.LogWarning("StringTable: textAsset is null! : " + strPath);
				return false;
			}
			
			if( string.IsNullOrEmpty( textAsset.text ) )
			{
				Debug.LogError("StringTable: Build file data is empty!");
				return false;
			}
			
			if( !Build(textAsset) ) { return false; }
		}
		catch(System.Exception e)
		{
			Debug.LogError("StringTable: " + e.Message);
			return false;
		}
		
		return true;
	}
	
}



