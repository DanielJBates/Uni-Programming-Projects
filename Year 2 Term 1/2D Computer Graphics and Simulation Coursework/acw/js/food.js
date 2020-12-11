class Food{
    constructor(pCanvas, pContext) {
        var x = Math.random() * ((pCanvas.width - 100) * 0.5 - ((-pCanvas.width + 100) * 0.5)) + ((-pCanvas.width + 100) * 0.5);
        var y = Math.random() * ((pCanvas.height - 100) * 0.5 - ((-pCanvas.height + 100) * 0.5)) + ((-pCanvas.height + 100) * 0.5);

        this.setPotistion(new Vector (x, y));
        this.setRotation(0 * Math.PI/180);
        this.setScale(new Vector(1,1))
        this.setRotationRate((5 * Math.PI/180) / 60);
        this.setScaleRate(0.05 / 60);
        this.setScaleState("Grow");
        this.setValue(10);
        this.initialiseSceneGraph(pContext);
    }

    //#region Getters & Setters
    getPosition() {
        return this.mPosition;
    }
    setPotistion(pPosition) {
        this.mPosition = pPosition;
    }
    getRotation() {
        return this.mRotation;
    }
    setRotation(pRotation) {
        this.mRotation = pRotation;
    }
    getScale() {
        return this.mScale;
    }
    setScale(pScale) {
        this.mScale = pScale;
    }
    getSceneGraph() {
        return this.mSceneGraph;
    }
    setSceneGraph(pSceneGraph) {
        this.mSceneGraph = pSceneGraph;
    }
    getRotationRate() {
        return this.mRotationRate;
    }
    setRotationRate(pRotationRate) {
        this.mRotationRate = pRotationRate;
    }
    getScaleRate() {
        return this.mScaleRate;
    }
    setScaleRate(pScaleRate) {
        this.mScaleRate = pScaleRate;
    }
    getScaleState() {
        return this.mScaleState;
    }
    setScaleState(pScaleState) {
        this.mScaleState = pScaleState;
    }
    getValue() {
        return this.mValue;
    }
    setValue(pValue) {
        this.mValue = pValue;
    }
    //#endregion

    updateFood(pDeltaTime) {
        var newRotation = this.getRotation() + (this.getRotationRate() * pDeltaTime);
        this.setRotation(newRotation);
        this.getSceneGraph().getChildAt(0).getChildAt(0).getChildAt(0).getChildAt(0).setTransform(Matrix.createRotation(this.getRotation()));

        var scaleRateVector = new Vector(this.getScaleRate(), this.getScaleRate())
        if(this.getScaleState() == "Grow") {
            var newScale = this.getScale().add(scaleRateVector.multiply(pDeltaTime));
            if(newScale.getX() >= 1.5) {
                this.setScaleState("Shrink");
            }
        }
        else if(this.getScaleState() == "Shrink") {
            var newScale = this.getScale().subtract(scaleRateVector.multiply(pDeltaTime));
            if(newScale.getX() <= 1) {
                this.setScaleState("Grow");
            }
        }
        this.setScale(newScale);
        this.getSceneGraph().getChildAt(0).getChildAt(0).getChildAt(0).getChildAt(1).setTransform(Matrix.createScale(this.getScale()));
    }
    initialiseSceneGraph(pContext) {
        var foodTranslationMatrix, foodRotationMatrix, foodScaleMatrix;

        foodTranslationMatrix = Matrix.createTranslation(this.getPosition());
        foodRotationMatrix = Matrix.createRotation(this.getRotation());
        foodScaleMatrix = Matrix.createScale(this.getScale());

        var foodTranslate, foodRotate, foodScale;

        foodTranslate = new Transform(foodTranslationMatrix);
        foodRotate = new Transform(foodRotationMatrix);
        foodScale = new Transform(foodScaleMatrix);

        foodTranslate.addChild(foodRotate);
        foodRotate.addChild(foodScale);

        var outerVectorArray = []
        outerVectorArray.push(new Vector(-6.25, -12.5));
        outerVectorArray.push(new Vector(6.25, -12.5));
        outerVectorArray.push(new Vector(12.5, -6.25));
        outerVectorArray.push(new Vector(12.5, 6.25));
        outerVectorArray.push(new Vector(6.25, 12.5));
        outerVectorArray.push(new Vector(-6.25, 12.5));
        outerVectorArray.push(new Vector(-12.5, 6.25));
        outerVectorArray.push(new Vector(-12.5, -6.25));
        
        var outerTransform = new Transform(foodRotationMatrix);
        outerTransform.addChild(new Geometry(pContext, new Polygon(outerVectorArray, '#ff0000', '#000000')));
        foodScale.addChild(outerTransform);

        var innerTransform = new Transform(foodScaleMatrix);
        innerTransform.addChild(new Geometry(pContext, new Circle(5, '#000000', '#000000')));
        foodScale.addChild(innerTransform);

        var foodTransforms = new Group();
        foodTransforms.addChild(foodTranslate);

        this.setSceneGraph(foodTransforms);
    }
}