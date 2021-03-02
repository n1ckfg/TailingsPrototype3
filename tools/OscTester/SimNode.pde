class SimNode {
  
  float target = 0.5;
  float val = 0.5;
  float speed = 0.1;
  float minRange = 0;
  float maxRange = 1;
  float minDistance = 0.01;
  float normVal = 0;
  
  SimNode() {
    init();
  }
  
  SimNode(float _minRange, float _maxRange) {
    minRange = _minRange;
    maxRange = _maxRange;
    init();
  }
  
  void init() {
    target = random(minRange, maxRange);
  }
  
  void update() {
    val = lerp(val, target, speed);
    normVal = map(val, minRange, maxRange, 0, 1);
    if (abs(val - target) < minDistance) init();
  }

}
