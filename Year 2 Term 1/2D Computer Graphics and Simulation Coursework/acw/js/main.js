function onLoad() {
    var mainCanvas, mainContext, visitor, rootSceneNode, lastTime, gameStart;
    function intialiseCanvasContext() {

        mainCanvas = document.getElementById('mainCanvas');
        if(!mainCanvas) {
            alert('Error: Canvas element not found');
            return;
        }
        mainContext = mainCanvas.getContext('2d');
        if(!mainContext) {
            alert('Error: Contetx not found');
            return;
        }

        lastTime = Date.now();
        
        gameStart = false;
        
        visitor = new RenderVistor();

        origin = new Vector(mainCanvas.width * 0.5, mainCanvas.height * 0.5);
        worldMatrix = Matrix.createTranslation(origin);

        rootSceneNode = new Group();
        worldMatrixTransform = new Transform(worldMatrix);
        sceneObjects = new Group();

        rootSceneNode.addChild(worldMatrixTransform);

        mainContext.lineWidth = 1;
        mainContext.lineJoin = 'round';

        var backgroundVectorArray = [];
        backgroundVectorArray.push(new Vector(-(mainCanvas.width), mainCanvas.height));
        backgroundVectorArray.push(new Vector(mainCanvas.width, mainCanvas.height));
        backgroundVectorArray.push(new Vector(mainCanvas.width, -(mainCanvas.height)));
        backgroundVectorArray.push(new Vector(-(mainCanvas.width), -(mainCanvas.height)));

        sceneObjects.addChild(new Geometry(mainContext, new Polygon(backgroundVectorArray, '#a9a9a9', '#000000')));

        snake = new Snake(new Vector(0, 0), (0 * Math.PI/180), new Vector(1, 1), mainContext);

        sceneObjects.addChild(snake.getSceneGraph());

        food = new Food(mainCanvas, mainContext);

        sceneObjects.addChild(food.getSceneGraph());

        score = new Score();

        sceneObjects.addChild(new Geometry(mainContext, score));
        
        worldMatrixTransform.addChild(sceneObjects);
    }
    function update(pDeltaTime) {
        window.addEventListener('keydown', (event) => {

            if(event.key === 'a' || event.key === 'ArrowLeft') {
                snake.updateRotation(pDeltaTime, 'left');
            } //left
            else if(event.key === 'd' || event.key === 'ArrowRight') {
                snake.updateRotation(pDeltaTime, 'right');
            } //right
        })
        snake.updatePosition(pDeltaTime);
        food.updateFood(pDeltaTime);
        if(food.foodHeadCollision(snake, mainCanvas)) {
            score.updateScore();
            snake.addBody(mainContext);
        }
    } 
    function draw() {
        visitor.visit(rootSceneNode);
    }
    function gameLoop() {
        var thisTime, deltaTime;

        window.addEventListener('keypress', (event) => {
            if(event.key === ' ' || event.key === 'Space') {
                gameStart = true;
            }
        })
        thisTime = Date.now();
        deltaTime = thisTime - lastTime;
        if(gameStart == true) {
            update(deltaTime);
        }
        draw();
        lastTime = thisTime;
        if(snake.checkGameOver(mainCanvas)) { 
            alert("Game Over Man, GAME OVER!" + "\r\n" + "Your Score Was " + score.getScore());
            return;
        }
        else if (!(snake.checkGameOver(mainCanvas))) {
            requestAnimationFrame(gameLoop);
        }
    }
    intialiseCanvasContext();
    gameLoop();
   
}
window.addEventListener('load', onLoad, false);