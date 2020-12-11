class Score {
    constructor() {
        this.setPosition(new Vector (500, -325));
        this.setScore(0);
    }
    //#region Getters & Setters
    getPosition() {
        return this.mPosition;
    }
    setPosition(pPosition) {
        this.mPosition = pPosition;
    }
    getScore() {
        return this.mScore;
    }
    setScore(pScore) {
        this.mScore = pScore;
    }
    //#endregion
    
    updateScore() {
        this.setScore(this.getScore() + 10);
    }
    draw(pContext) {
        pContext.font = "15pt Comic Sans";
        pContext.fillStyle = "#000000";
        pContext.fillText("Score:" + this.getScore(), this.getPosition().getX(), this.getPosition().getY());
    }
}