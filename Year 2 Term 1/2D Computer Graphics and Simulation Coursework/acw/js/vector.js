class Vector {
    constructor(pX, pY, pZ) {
        this.setX(pX);
        this.setY(pY);
        this.setZ(pZ);
    }
    //#region Getters & Setters
    getX() {
        return this.mX;
    }
    setX(pX) {
        this.mX = pX;
    }
    getY() {
        return this.mY;
    }
    setY(pY) {
        this.mY = pY;
    }
    getZ() {
        return this.mZ;
    }
    setZ(pZ) {
        if(pZ == null) {
            pZ = 1;
        }
        this.mZ = pZ;
    }
    //#endregion
    add(pVector) {
        var tempX, tempY, tempZ, productVector; 
        
        tempX = this.getX() + pVector.getX();
        tempY = this.getY() + pVector.getY();
        tempZ = this.getZ() + pVector.getZ();

        productVector = new Vector(tempX, tempY, tempZ);

        return productVector;
    }
    subtract(pVector) {
        var tempX, tempY, tempZ, productVector;

        tempX = this.getX() - pVector.getX();
        tempY = this.getY() - pVector.getY();
        tempZ = this.getZ() - pVector.getZ();

        productVector = new Vector(tempX, tempY, tempZ);

        return productVector;
    }
    multiply(pScalar) {
        var tempX, tempY, tempZ, productVector;

        tempX = this.getX() * pScalar;
        tempY = this.getY() * pScalar;
        tempZ = this.getZ() * pScalar;

        productVector = new Vector(tempX, tempY, tempZ);

        return productVector;
    }
    divide(pScalar) {
        var tempX, tempY, tempZ, productVector;

        tempX = this.getX() / pScalar;
        tempY = this.getY() / pScalar;
        tempZ = this.getZ() / pScalar;

        productVector = new Vector(tempX, tempY, tempZ);

        return productVector;
    }
    magnitude() {
        var tempX, tempY, tempZ, magnitude;

        tempX = this.getX() * this.getX();
        tempY = this.getY() * this.getY();
        tempZ = this.getZ() * this.getZ();

        magnitude = Math.sqrt(tempX + tempY + tempZ);

        return magnitude;
    }
    normalise() {
        var tempX, tempY, tempZ, magnitude, productVector;

        magnitude = this.magnitude();

        tempX = this.getX() / magnitude;
        tempY = this.getY() / magnitude;
        tempZ = this.getZ() / magnitude;

        productVector = new Vector(tempX, tempY, tempZ);

        return productVector;
    }
    limitTo(pScalar) {
        var tempMagnitude = this.magnitude();

        if(tempMagnitude > pScalar) {
            var productVector = this.normalise();
            productVector.setX(productVector.getX() * pScalar);
            productVector.setY(productVector.getY() * pScalar);
            productVector.setZ(productVector.getZ() * pScalar);

            return productVector;
        }
        else {
            return this;
        }
    }
    dotProduct(pVector) {
        var tempX, tempY, tempZ, productScalar;

        tempX = this.getX() * pVector.getX();
        tempY = this.getY() * pVector.getY();
        tempZ = this.getZ() * pVector.getZ();

        productScalar = tempX + tempY + tempZ;

        return productScalar;
    }
    interpolate(pVector, pScalar) {
        var productVector, interpolatedVector;

        productVector = (pVector.subtract(this)).multiply(pScalar);
        interpolatedVector = this.add(productVector);

        return interpolatedVector;
    }
    rotate(pScalar) {
        var tempX, tempY, tempZ, productVector;

        tempX = Math.cos(pScalar) * this.getX() - Math.sin(pScalar) * this.getY();
        tempY = Math.sin(pScalar) * this.getX() + Math.cos(pScalar) * this.getY();
        tempZ = this.getZ();

        productVector = new Vector(tempX, tempY, tempZ);

        return productVector;
    }
    angleBetween(pVector) {
        var productScalar;

        productScalar = Math.acos((this.dotProduct(pVector)) / (this.magnitude() * pVector.magnitude()));

        return productScalar;
    }
}