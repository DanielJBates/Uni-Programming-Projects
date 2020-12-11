class Snake {
    constructor(pPosition, pRotation, pScale, pContext) {
        this.mChildren = [];

        this.setMass(1);
        this.setVelocity(new Vector(0, -5 / 60));
        this.setAcceleration(1);
        this.setRotationRate((0.03 * Math.PI/180) / 60)
        this.setPosition(pPosition);
        this.setRotation(pRotation);
        this.setScale(pScale);
        this.initialiseSceneGraph(pContext);
    }
    //#region Getters & Setters
    getPosition() {
        return this.mPosition;
    }
    setPosition(pPosition) {
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
    getHeadTranslate() {
        return this.mHeadTranslate;
    }
    setHeadTranslate(pTranslation) {
        this.mHeadTranslate = pTranslation;
    }
    getHeadRotation() {
        return this.mHeadRotate;
    }
    setHeadRotate(pRotation) {
        this.mHeadRotate = pRotation
    }
    getBodyTranslate(pIndex) {
        return this.mBodyTransforms.getChildAt(pIndex);
    }
    getBodyTransforms() {
        return this.mBodyTransforms;
    }
    setBodyTransform(pGroup) {
        this.mBodyTransforms = pGroup;
    }
    getMass() {
        return this.mMass;
    }
    setMass(pMass) {
        this.mMass = pMass;
    }
    getAcceleration() {
        return this.mAcceleration;
    }
    setAcceleration(pAcceleration) {
        this.mAcceleration = pAcceleration;
    }
    getVelocity() {
        return this.mVelocity;
    }
    setVelocity(pVelocity) {
        this.mVelocity = pVelocity;
    }
    getRotationRate() {
        return this.mRotationRate;
    }
    setRotationRate(pRotationRate) {
        this.mRotationRate = pRotationRate;
    }
    getSceneGraph() {
        return this.mSceneGraph
    }
    setSceneGraph(pSceneGraph) {
        this.mSceneGraph = pSceneGraph;
    }
    getNumberOfChildren() {
        return this.mChildren.length;
    }
    getChildAt(pIndex) {
        return this.mChildren[pIndex];
    }
    setChildAt(pIndex, pVector) {
        this.mChildren[pIndex] = pVector;
    }
    addChild(pChild) {
        this.mChildren.push(pChild);
    }
    //#endregion
    
    initialiseSceneGraph(pContext) {
        var snakeTranslationMatrix, snakeRotationMatrix, snakeScaleMatrix;

        snakeTranslationMatrix = Matrix.createTranslation(this.getPosition());
        snakeRotationMatrix = Matrix.createRotation(this.getRotation());
        snakeScaleMatrix = Matrix.createScale(this.getScale());

        var snakeTranslate, snakeRotate, snakeScale;

        snakeTranslate = new Transform(snakeTranslationMatrix);
        snakeRotate = new Transform(snakeRotationMatrix);
        snakeScale = new Transform(snakeScaleMatrix);

        snakeTranslate.addChild(snakeRotate);
        snakeRotate.addChild(snakeScale);

        //#region Head set up
        var headVectorArray = [];
        headVectorArray.push(new Vector(0, -15));
        headVectorArray.push(new Vector(15, 15));
        headVectorArray.push(new Vector(-15, 15));

        this.addChild(new Vector(0, 0));
        var headTranslationMatrix = Matrix.createTranslation(this.getChildAt(0));
        this.setHeadTranslate(new Transform(headTranslationMatrix));

        var headRotationMatrix = Matrix.createRotation(0 * Math.PI/180)
        this.setHeadRotate(new Transform(headRotationMatrix));
        this.getHeadTranslate().addChild(this.getHeadRotation());

        this.getHeadRotation().addChild(new Geometry(pContext, new Polygon(headVectorArray, '#29a329', '#000000')));
        snakeScale.addChild(this.getHeadTranslate());
        //#endregion

        //#region Body set up
        var bodyTransforms = new Group();
        this.setBodyTransform(bodyTransforms);

        for(var i = 0; i < 2; i += 1) {
            this.addChild(new Vector(0, 35 * (i + 1)));
            var bodyTranslationMatrix = Matrix.createTranslation(this.getChildAt(i + 1));
            var bodyTransform = new Transform(bodyTranslationMatrix);
            bodyTransform.addChild(new Geometry(pContext, new Circle(15, '#29a329', '#000000')));
            bodyTransforms.addChild(bodyTransform);
        } 
        snakeScale.addChild(bodyTransforms);
        //#endregion
        var snakeTransforms = new Group();
        snakeTransforms.addChild(snakeTranslate);

        this.setSceneGraph(snakeTransforms);
    }

    //#region Updates
    updateRotation(pDeltaTime, pDirection) {
        if(pDirection == 'right') {
            var newRotation = this.getRotation() + (this.getRotationRate() * pDeltaTime);
            if(newRotation > (Math.PI * 2)) {
                newRotation = newRotation - (Math.PI * 2);
            }
            this.setRotation(newRotation);
            var newRotationMatrix = Matrix.createRotation(newRotation);
            this.getHeadRotation().setTransform(newRotationMatrix);
        }
        else if(pDirection == 'left') {
            var newRotation = this.getRotation() - (this.getRotationRate() * pDeltaTime);
            if(newRotation <  0) {
                newRotation = newRotation + (Math.PI * 2);
            }
            this.setRotation(newRotation);
            var newRotationMatrix = Matrix.createRotation(newRotation);
            this.getHeadRotation().setTransform(newRotationMatrix);
        }
    }
    updatePosition(pDeltaTime) { 
        //#region Head Position Update
        var rotatedVelocity = this.getVelocity().rotate(this.getRotation());
        rotatedVelocity = rotatedVelocity.multiply(pDeltaTime);

        var newPosition = this.getChildAt(0).add(rotatedVelocity);

        this.getChildAt(0).setX(newPosition.getX());
        this.getChildAt(0).setY(newPosition.getY());

        var newTranslationMatrix = Matrix.createTranslation(newPosition);
        this.getHeadTranslate().setTransform(newTranslationMatrix);
        //#endregion

        //#region Body Positions Update
        for(var i = 1; i < this.getNumberOfChildren(); i += 1) {
            var tempVector = this.getChildAt(i).subtract(this.getChildAt(i - 1));
            tempVector = tempVector.normalise();
            var newPosition = this.getChildAt(i - 1).add(tempVector.multiply(35));
            this.setChildAt(i, newPosition);

            this.getBodyTranslate(i - 1).setTransform(Matrix.createTranslation(this.getChildAt(i)));       
        }
        //#endregion        
    }
    //#endregion

    //#region Collisions
    checkGameOver(pCanvas) {
        if(this.headBodyCollision() || this.headBoundaryCollision(pCanvas)) {
            return true;
        }
        else {
            return false;
        }
    }    
    headBodyCollision() {
        for(var i = 1; i < this.getNumberOfChildren(); i += 1) {
            var dx = (this.getChildAt(0).getX() - this.getChildAt(i).getX())
            var dy = (this.getChildAt(0).getY() - this.getChildAt(i).getY())
            var distance = Math.sqrt((dx * dx) + (dy * dy))

            if(distance < 15) {
                return true;
            }
        }

        return false;
    }
    headBoundaryCollision(pCanvas) {
        if(this.getChildAt(0).getX() >= (pCanvas.width * 0.5) || this.getChildAt(0).getX() <= -(pCanvas.width * 0.5) || this.getChildAt(0).getY() >= (pCanvas.height * 0.5) || this.getChildAt(0).getY() <= -(pCanvas.height * 0.5)) {
            return true;
        }
        else {
            return false;
        }
    }
    //#endregion

    addBody(pContext) {
        this.addChild(new Vector(0, (35 * this.getNumberOfChildren() - 1)));
        var bodyTranslationMatrix = Matrix.createTranslation(this.getChildAt(this.getNumberOfChildren() - 1));
        var bodyTransform = new Transform(bodyTranslationMatrix);
        bodyTransform.addChild(new Geometry(pContext, new Circle(15, '#29a329', '#000000')));
        this.getBodyTransforms().addChild(bodyTransform);
    }
}