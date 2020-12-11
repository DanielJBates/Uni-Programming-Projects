class Matrix {
    constructor(pX1, pY1, pZ1, pX2, pY2, pZ2, pX3, pY3, pZ3) {
        this.elements = new Array(3);
        for (var i = 0; i < this.elements.length; i += 1) {
            this.elements[i] = new Array(3);
        }

        this.elements[0][0] = pX1;
        this.elements[0][1] = pY1;
        this.elements[0][2] = pZ1;
        this.elements[1][0] = pX2;
        this.elements[1][1] = pY2;
        this.elements[1][2] = pZ2;
        this.elements[2][0] = pX3;
        this.elements[2][1] = pY3;
        this.elements[2][2] = pZ3;
    }
    getElement(pRow, pColumn) {
        return this.elements[pRow][pColumn];
    }
    static createIdentity() {
        var identityMatrix = new Matrix(1,0,0,0,1,0,0,0,1);
        return identityMatrix;
    }
    static createTranslation(pVector) {
        var translationMatrix = new Matrix(1,0,pVector.getX(),0,1,pVector.getY(),0,0,1);
        return translationMatrix;
    }
    static createScale(pVector) {
        var scaleMatrix = new Matrix(pVector.getX(),0,0,0,pVector.getY(),0,0,0,1);
        return scaleMatrix;
    }
    static createRotation(pScalar) {
        var rotationMatrix = new Matrix(Math.cos(pScalar),-(Math.sin(pScalar)),0,Math.sin(pScalar),Math.cos(pScalar),0,0,0,1);
        return rotationMatrix;
    }
    multiply(pMatrix) {
        var tempX1, tempY1, tempZ1, tempX2, tempY2, tempZ2, tempX3, tempY3, tempZ3;

        tempX1 = (this.getElement(0,0) * pMatrix.getElement(0,0)) + (this.getElement(0,1) * pMatrix.getElement(1,0)) + (this.getElement(0,2) * pMatrix.getElement(2,0));
        tempY1 = (this.getElement(0,0) * pMatrix.getElement(0,1)) + (this.getElement(0,1) * pMatrix.getElement(1,1)) + (this.getElement(0,2) * pMatrix.getElement(2,1));
        tempZ1 = (this.getElement(0,0) * pMatrix.getElement(0,2)) + (this.getElement(0,1) * pMatrix.getElement(1,2)) + (this.getElement(0,2) * pMatrix.getElement(2,2));
        tempX2 = (this.getElement(1,0) * pMatrix.getElement(0,0)) + (this.getElement(1,1) * pMatrix.getElement(1,0)) + (this.getElement(1,2) * pMatrix.getElement(2,0));
        tempY2 = (this.getElement(1,0) * pMatrix.getElement(0,1)) + (this.getElement(1,1) * pMatrix.getElement(1,1)) + (this.getElement(1,2) * pMatrix.getElement(2,1));
        tempZ2 = (this.getElement(1,0) * pMatrix.getElement(0,2)) + (this.getElement(1,1) * pMatrix.getElement(1,2)) + (this.getElement(1,2) * pMatrix.getElement(2,2));
        tempX3 = (this.getElement(2,0) * pMatrix.getElement(0,0)) + (this.getElement(2,1) * pMatrix.getElement(1,0)) + (this.getElement(2,2) * pMatrix.getElement(2,0));
        tempY3 = (this.getElement(2,0) * pMatrix.getElement(0,1)) + (this.getElement(2,1) * pMatrix.getElement(1,1)) + (this.getElement(2,2) * pMatrix.getElement(2,1));
        tempZ3 = (this.getElement(2,0) * pMatrix.getElement(0,2)) + (this.getElement(2,1) * pMatrix.getElement(1,2)) + (this.getElement(2,2) * pMatrix.getElement(2,2));

        var productMatrix = new Matrix(tempX1, tempY1, tempZ1, tempX2, tempY2, tempZ2, tempX3, tempY3, tempZ3);
        return productMatrix;
    }
    multiplyVector(pVector) {
        var tempX, tempY, tempZ;

        tempX = (this.getElement(0,0) * pVector.getX()) + (this.getElement(0,1) * pVector.getY()) + (this.getElement(0,2) * pVector.getZ());
        tempY = (this.getElement(1,0) * pVector.getX()) + (this.getElement(1,1) * pVector.getY()) + (this.getElement(1,2) * pVector.getZ());
        tempZ = (this.getElement(2,0) * pVector.getX()) + (this.getElement(2,1) * pVector.getY()) + (this.getElement(2,2) * pVector.getZ());

        var productVector = new Vector(tempX, tempY, tempZ);
        return productVector;
    }
    setTransform(pContext) {
        pContext.setTransform(this.getElement(0,0),this.getElement(1,0),this.getElement(0,1),this.getElement(1,1),this.getElement(0,2),this.getElement(1,2));
    }
    transform(pContext) {
        pContext.transform(this.getElement(0,0),this.getElement(1,0),this.getElement(0,1),this.getElement(1,1),this.getElement(0,2),this.getElement(1,2));
    }
    alert() {
        for(i = 0; i < this.elements.length; i += 1) {
            for(j = 0; j < this.elements[i].length; j += 1) {
                print(this.elements[i][j]);
            }
        }
    }
}