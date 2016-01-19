using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WMG_Hierarchical_Tree : WMG_Graph_Manager {

	public GameObject nodeParent;
	public GameObject linkParent;
	
	public Object defaultNodePrefab;
	public Object linkPrefab;
	
	public int numNodes;
	public int numLinks;
	
	public List<Object> nodePrefabs;
	public List<int> nodeColumns;
	public List<int> nodeRows;
	public List<int> linkNodeFromIDs;
	public List<int> linkNodeToIDs;
	
	public Object invisibleNodePrefab;
	public int numInvisibleNodes;
	public List<int> invisibleNodeColumns;
	public List<int> invisibleNodeRows;

	// Determines distance between each column and row
	public float gridLengthX { get {return _gridLengthX;} 
		set {
			if (_gridLengthX != value) {
				_gridLengthX = value;
				treeC.Changed();
			}
		}
	}
	public float gridLengthY { get {return _gridLengthY;} 
		set {
			if (_gridLengthY != value) {
				_gridLengthY = value;
				treeC.Changed();
			}
		}
	}
	
	// Determines size of the nodes. The nodeRadius and squareNodes determine how the links connect to the nodes.
	public int nodeWidthHeight { get {return _nodeWidthHeight;} 
		set {
			if (_nodeWidthHeight != value) {
				_nodeWidthHeight = value;
				treeC.Changed();
			}
		}
	}
	public float nodeRadius { get {return _nodeRadius;} 
		set {
			if (_nodeRadius != value) {
				_nodeRadius = value;
				treeC.Changed();
			}
		}
	}
	public bool squareNodes { get {return _squareNodes;} 
		set {
			if (_squareNodes != value) {
				_squareNodes = value;
				treeC.Changed();
			}
		}
	}

	[SerializeField] private float _gridLengthX;
	[SerializeField] private float _gridLengthY;
	[SerializeField] private int _nodeWidthHeight;
	[SerializeField] private float _nodeRadius;
	[SerializeField] private bool _squareNodes;

	private List<WMG_Change_Obj> changeObjs = new List<WMG_Change_Obj>();
	private WMG_Change_Obj treeC = new WMG_Change_Obj();

	private List<GameObject> treeNodes = new List<GameObject>();
	private List<GameObject> treeLinks = new List<GameObject>();
	private List<GameObject> treeInvisibleNodes = new List<GameObject>();

	private bool hasInit;
	
	void Start() {
		Init ();
		PauseCallbacks();
		CreateNodes();
		CreatedLinks();
		treeC.Changed();
	}

	public void Init() {
		if (hasInit) return;
		hasInit = true;
		
		changeObjs.Add(treeC);

		treeC.OnChange += refresh;
		
		PauseCallbacks();
	}

	void Update() {
		Refresh();
	}
	
	public void Refresh() {
		ResumeCallbacks();
		PauseCallbacks();
	}
	
	void PauseCallbacks() {
		for (int i = 0; i < changeObjs.Count; i++) {
			changeObjs[i].changesPaused = true;
			changeObjs[i].changePaused = false;
		}
	}
	
	void ResumeCallbacks() {
		for (int i = 0; i < changeObjs.Count; i++) {
			changeObjs[i].changesPaused = false;
			if (changeObjs[i].changePaused) changeObjs[i].Changed();
		}
	}

	public List<GameObject> getNodesInRow(int rowNum) {
		List<GameObject> returnList = new List<GameObject>();
		for (int i = 0; i < treeNodes.Count; i++) {
			if (Mathf.Approximately(getSpritePositionY(treeNodes[i]), -rowNum*gridLengthY)) returnList.Add(treeNodes[i]);
		}
		return returnList;
	}
	
	public List<GameObject> getNodesInColumn(int colNum) {
		List<GameObject> returnList = new List<GameObject>();
		for (int i = 0; i < treeNodes.Count; i++) {
			if (Mathf.Approximately(getSpritePositionX(treeNodes[i]), colNum*gridLengthX)) returnList.Add(treeNodes[i]);
		}
		return returnList;
	}
	
	void refresh() {
		// Update node positions
		for (int i = 0; i < treeNodes.Count; i++) {
			changeSpriteWidth(treeNodes[i],nodeWidthHeight);
			changeSpriteHeight(treeNodes[i],nodeWidthHeight);
			treeNodes[i].GetComponent<WMG_Node>().radius = nodeRadius;
			treeNodes[i].GetComponent<WMG_Node>().isSquare = squareNodes;
			float xPos = nodeColumns[i]*gridLengthX;
			float yPos = nodeRows[i]*gridLengthY;
			treeNodes[i].GetComponent<WMG_Node>().Reposition(xPos, -yPos);
		}
		// Update invisible node positions
		for (int i = 0; i < treeInvisibleNodes.Count; i++) {
			changeSpritePivot(treeInvisibleNodes[i],WMG_GUI_Functions.WMGpivotTypes.Center);
			changeSpriteWidth(treeInvisibleNodes[i],nodeWidthHeight);
			changeSpriteHeight(treeInvisibleNodes[i],nodeWidthHeight);
			float xPos = invisibleNodeColumns[i]*gridLengthX;
			float yPos = invisibleNodeRows[i]*gridLengthY;
			treeInvisibleNodes[i].GetComponent<WMG_Node>().Reposition(xPos, -yPos);
		}
	}
	
	public void CreateNodes() {
		// Create nodes based on numNodes
		for (int i = 0; i < numNodes; i++) {
			if (treeNodes.Count <= i) {
				Object nodePrefab = defaultNodePrefab;
				if (nodePrefabs.Count > i) nodePrefab = nodePrefabs[i];
				WMG_Node curNode = CreateNode(nodePrefab, nodeParent).GetComponent<WMG_Node>();
				treeNodes.Add(curNode.gameObject);
			}
		}
		// Create invisible nodes
		for (int i = 0; i < numInvisibleNodes; i++) {
			if (treeInvisibleNodes.Count <= i) {
				WMG_Node curNode = CreateNode(invisibleNodePrefab, nodeParent).GetComponent<WMG_Node>();
				treeInvisibleNodes.Add(curNode.gameObject);
			}
		}
	}
	
	public void CreatedLinks() {
		// Create links based on numLinks
		for (int i = 0; i < numLinks; i++) {
			if (treeLinks.Count <= i) {
				GameObject fromNode = null;
				if (linkNodeFromIDs[i] > 0) { // Regular node
					fromNode = treeNodes[linkNodeFromIDs[i]-1];
				}
				else { // Invisible node
					fromNode = treeInvisibleNodes[linkNodeFromIDs[i]+1];
				}
				GameObject toNode = null;
				if (linkNodeToIDs[i] > 0) { // Regular node
					toNode = treeNodes[linkNodeToIDs[i]-1];
				}
				else { // Invisible node
					toNode = treeInvisibleNodes[linkNodeToIDs[i]+1];
				}
				treeLinks.Add(CreateLinkNoRepos(fromNode.GetComponent<WMG_Node>(), toNode, linkPrefab, linkParent));
			}
		}
	}
}
