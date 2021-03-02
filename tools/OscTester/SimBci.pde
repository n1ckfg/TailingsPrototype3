class SimBci {
  
  SimNode[] nodes = new SimNode[6];
  
  SimBci() {
    // 1. Raw input signal, positive or negative: -0.25–0.25
    nodes[0] = new SimNode(-0.25, 0.25);

    // 2. Cooked input signal, normalized: 0–1
    nodes[1] = new SimNode(0, 1);

    // 3. Heart rate (bpm): 40 athletic, 60–100 normal 
    nodes[2] = new SimNode(60, 100);

    // 4. R to r, distance peak to peak--can extrapolate trends: typically 0.6–0.9
    nodes[3] = new SimNode(0.6, 0.9);

    // 5. Respiration, cycle state: 0.1–0.6
    nodes[4] = new SimNode(0.1, 0.6);

    // 6. Respiration rate (rr): 12–18 normal
    nodes[5] = new SimNode(12, 18);
  }
  
  void update() {
    OscMessage msg = new OscMessage("/simbci");

    for (int i=0; i<nodes.length; i++) {
      nodes[i].update();
      msg.add(nodes[i].normVal);
    }
    
    oscP5.send(msg, myRemoteLocation);
  }

}
